using CloudinaryDotNet.Actions;
using Firebase.Auth.Repository;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Repositories.DTO;
using Repositories.Implements;
using Repositories.Interfaces;
using Repositories.Models;
using Services.Common;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implements
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _account;
        private readonly IImageService _image;
        private readonly IConfiguration _configuration;
        public AccountService(IAccountRepository account, IImageService image, IConfiguration configuration)
        {
            _account = account;
            _image = image;
            _configuration = configuration;
        }

        public async Task<Account> GetAccountByEmailAsync(string email) => await _account.GetAccountByEmailAsync(email);
        public async Task<Account> GetAccountByPhoneAsync(double phone) => await _account.GetAccountByPhoneAsync(phone);


        public async Task<string> AutoGenerateAccountId()
        {
            string newAccountId = "";
            string latestAccountId = await _account.GetLatestAccountIdAsync();
            if (string.IsNullOrEmpty(latestAccountId))
            {
                newAccountId = "AC00000001";
            }
            else
            {
                int numericpart = int.Parse(latestAccountId.Substring(2));
                int newnumericpart = numericpart + 1;
                newAccountId = $"AC{newnumericpart:d8}";
            }
            return newAccountId;
        }
        public async Task<string> AutoGenerateCustomerId()
        {
            string newCustomerId = "";
            string latestCustomerId = await _account.GetLatestCustomerIdAsync();
            if (string.IsNullOrEmpty(latestCustomerId))
            {
                newCustomerId = "C000000001";
            }
            else
            {
                int numericpart = int.Parse(latestCustomerId.Substring(1));
                int newnumericpart = numericpart + 1;
                newCustomerId = $"C{newnumericpart:d9}";
            }
            return newCustomerId;
        }
        public string GenerateJwtToken(string email, int Role, double expirationMinutes)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            // add role to author
            var role = Role switch
            {
                1 => "Admin",
                2 => "Customer",
                3 => "Staff"
            };
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role)// claim role
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(expirationMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<dynamic> RegisterAsACustomerAsync(RegisterDTO accountDTO)
        {
            try
            {

                var acc = new Account
                {
                    AccountId = await AutoGenerateAccountId(),
                    Email = accountDTO.Email,
                    Password = Encryption.Encrypt(accountDTO.Password),
                    Role = "Customer",
                    FullName = accountDTO.Fullname,
                    UserName = accountDTO.Username,
                    Phone = accountDTO.Phone,
                    Birthday = accountDTO.Birthday,
                    Address = accountDTO.Address,
                    Gender = accountDTO.Gender,
                    Status = "Active",
                    //Status = "PendingToConfirm",

                };

                // Save the acc
                int rs1 = await _account.CreateAccountAsync(acc);
                var avatarUrl = await _image.GenerateAndUploadAvatarAsync(acc.FullName);
                var customer = new Customer
                {
                    CustomerId = await AutoGenerateCustomerId(),
                    AccountId = acc.AccountId,
                    Avatar = avatarUrl,
                };
                // Save customer
                int rs2 = await _account.CreateCustomerAsync(customer);
                // Generate JWT token
                var token = GenerateJwtToken(accountDTO.Email, 2, 5);

                // Send the confirmation email

                return rs1 & rs2;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }


        // FOR LOGIN
        public async Task<Account> CheckLogin(string email, string password)
        {
            var acc = await _account.GetAccountByEmailAsync(email);
            if (acc == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            bool isPasswordValid = Encryption.VerifyPassword(password, acc.Password);
            if (isPasswordValid)
            {
                return acc;
            }
            else
            {
                throw new UnauthorizedAccessException("Invalid  password");
            }
        }



        // FOR SIGN-IN BY GOOGLE
        public async Task<AuthResponse> GetFirebaseToken(string firebaseToken)
        {
            try
            {
                FirebaseToken decryptedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(firebaseToken);

                string uId = decryptedToken.Uid;
                // get the acc id of the acc which is the token belongs to
                UserRecord record = await FirebaseAuth.DefaultInstance.GetUserAsync(uId); // get information of the acc that has this uId
                string email = record.Email;
                string fullName = record.DisplayName;

                Account acc = await _account.GetAccountByEmailAsync(email);

                AuthResponse response = new()
                {
                    Email = email,
                    FullName = fullName
                };

                if (acc == null)
                {
                    throw new Exception("This account doesn't exist. Please register to gain access to our website.");
                }
                else if (acc.Email != null && acc.Password == "GOOGLE_SIGNIN")
                {
                    if (acc.FullName == null && acc.Birthday == null && acc.Address == null && acc.Phone == null)
                    {
                        throw new Exception("This account cannot sign in by google. Try another method.");

                    }
                    //throw new Exception("Login successfull!");
                }
                List<Claim> authClaims = new List<Claim>
                {
                    //new Claim("UserId", acc.UserId),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, fullName),
                };
                //var token = TokenHelper.Instance.CreateToken(authClaims, _configuration);
                var token = GenerateJwtToken(email, 2, 30);
                response.Token = token;
                response.Role = acc.Role;
                return response;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        // FOR SIGN-UP BY GOOGLE
        public async Task<dynamic> SignUpByGoogleAsync(string firebaseToken, long phone, DateTime birthday, string address, string gender)
        {
            try
            {
                FirebaseToken decryptedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(firebaseToken);
                string uId = decryptedToken.Uid;

                UserRecord record = await FirebaseAuth.DefaultInstance.GetUserAsync(uId);
                string email = record.Email;
                string fullName = record.DisplayName;

                Account acc = await _account.GetAccountByEmailAsync(email);

                if (acc != null)
                {
                    throw new Exception("This account is already registered.");
                }

                acc = new Account
                {
                    AccountId = await AutoGenerateAccountId(),
                    Email = email,
                    Password = "GOOGLE_SIGNIN",
                    FullName = fullName,
                    UserName = fullName,
                    Phone = phone,
                    Birthday = birthday,
                    Address = address,
                    Gender = gender,
                    Status = "Active",
                    Role = "Customer",
                };

                await _account.CreateAccountAsync(acc);
                Customer cus = new Customer
                {

                    CustomerId = await AutoGenerateCustomerId(),
                    AccountId = acc.AccountId,
                    Avatar = record.PhotoUrl
                };
                await _account.CreateCustomerAsync(cus);
                AuthResponse authResponse = new()
                {
                    Email = email,
                    FullName = fullName,
                    Role = acc.Role,
                };

                List<Claim> authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, fullName),
        };
                var token = GenerateJwtToken(email, 2, 60);
                authResponse.Token = token;
                authResponse.Role = acc.Role;

                return authResponse;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        // Profile
        public async Task<dynamic> GetUserAccountAsync(string accessToken)
        {
            string accountEmail = TokenDecoder.GetEmailFromToken(accessToken);
            var account = await _account.GetAccountByEmailAsync(accountEmail);
            var customer = await _account.GetCustomerByAccountIdAsync(account.AccountId);
            return account;
        }

        public async Task<dynamic> GetCustomerAsync(string accountId)
        {

            var customer = await _account.GetCustomerByAccountIdAsync(accountId);
            return customer;
        }

        public async Task<bool> UpdateAccountProfileByAdminAsync(string accessToken, UpdateAccountProfileDTO accountProfile)
        {
            var accountEmail = TokenDecoder.GetEmailFromToken(accessToken);
            var acc = await _account.GetAccountByEmailAsync(accountEmail);
            if (acc == null)
            {
                return false;
            }

            // if users do not enter any items, these items will keep the initial data in db
            if (!string.IsNullOrEmpty(accountProfile.Fullname))
            {
                acc.FullName = accountProfile.Fullname;
            }
            if (!string.IsNullOrEmpty(accountProfile.Email))
            {
                acc.Email = accountProfile.Email;
            }
            if (!string.IsNullOrEmpty(accountProfile.Username))
            {
                acc.UserName = accountProfile.Username;
            }
            if (accountProfile.Phone == 0)
            {
                acc.Phone = accountProfile.Phone;
            }
            if (!string.IsNullOrEmpty(accountProfile.Gender))
            {
                acc.Gender = accountProfile.Gender;
            }
            if (!string.IsNullOrEmpty(accountProfile.Address))
            {
                acc.Address = accountProfile.Address;
            }

            Customer cus = await _account.GetCustomerByAccountIdAsync(acc.AccountId);
            string attachmentUri = null;
            //Upload each attachment file
            if (accountProfile.AttachmentFile != null && !string.IsNullOrEmpty(cus.Avatar))
            {
                await _image.DeleteImageAsync(cus.Avatar); // Delete the old avatar from Cloudinary
                var attachment = await _image.UploadImageAsync(accountProfile.AttachmentFile, "avatars");
                attachmentUri = attachment.SecureUri.AbsoluteUri;
                cus.Avatar = attachmentUri;
            }

            await _account.UpdateCustomer(cus);
            return await _account.UpdateAccount(acc);
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var acc = await _account.GetAccountByEmailAsync(email);
            if (acc == null)
            {
                return false;
            }
            var role = acc.Role switch
            {
                "Admin" => 1,
                "Customer" => 2,
                "Staff" => 3
            };
            var token = GenerateJwtToken(acc.Email, role, 5);

            //await SendResetPasswordEmailAsync(email, token);
            //send link front to email

            return true;
        }

        //    public async Task SendResetPasswordEmailAsync(string email, string token)
        //    {
        //        var resetLink = $"https://cursus-online-hung.web.app/resetpassword/{token}";

        //        var emailContent = $@"
        //    <html>
        //<head>
        //    <style>
        //        .email-container {{
        //            font-family: Arial, sans-serif;
        //            color: #333;
        //            line-height: 1.6;
        //        }}
        //        .header {{
        //            background-color: #4CAF50;
        //            color: white;
        //            padding: 10px;
        //            text-align: center;
        //        }}
        //        .content {{
        //            padding: 20px;
        //        }}
        //        .content a{{
        //            background-color: #4CAF50;
        //            color: white !important;
        //            padding: 10px 20px;
        //            text-align: center;
        //            text-decoration: none;
        //            display: inline-block;
        //            margin: 10px 0;
        //            border-radius: 5px;
        //        }}
        //        .footer {{
        //            margin-top: 20px;
        //            text-align: center;
        //            font-size: 12px;
        //            color: #999;
        //        }}
        //    </style>
        //</head>
        //<body>
        //    <div class='email-container'>
        //        <div class='header'>
        //            <h1>Reset Your Password</h1>
        //        </div>
        //        <div class='content'>
        //            <p>Dear User,</p>
        //            <p>We received a request to reset your password for <strong>Cursus - Online Course Management</strong>.</p>
        //            <p>Please click the button below to reset your password:</p>
        //            <p>
        //                <a href='{resetLink}' class='reset-button'>Reset Your Password</a>
        //            </p>
        //            <p>If you did not request a password reset, please ignore this email.</p>
        //            <p>Best regards,</p>
        //            <p><strong>The Cursus Team</strong></p>
        //        </div>
        //        <div class='footer'>
        //            <p>Cursus - Online Course Management</p>
        //            <p>&copy; 2024 Cursus. All rights reserved.</p>
        //        </div>
        //    </div>
        //</body>
        //</html>";
        //        await _emailService.SendEmailAsync(email, "Email reset-password", emailContent);

        //    }


        public async Task<ApiResponse> ResetPasswordAsync(string token, string newPassword)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                ClaimsPrincipal principal;
                SecurityToken validatedToken;

                try
                {
                    principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
                }
                catch (SecurityTokenExpiredException)
                {
                    // Token hết hạn, gửi lại email reset password với token mới
                    var expiredToken = new JwtSecurityToken(token);
                    var expiredEmail = expiredToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
                    var newToken = GenerateJwtToken(expiredEmail, 2, 5); // Tạo token mới có hạn trong 5 phút
                    //await SendResetPasswordEmailAsync(expiredEmail, newToken);
                    return new ApiResponse { StatusCode = 401, Message = "Token expired. " };
                    //A new password reset link has been sent to your email.
                }

                // Lấy email từ token đã xác thực
                var jwtToken = (JwtSecurityToken)validatedToken;
                var email = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;

                var acc = await _account.GetAccountByEmailAsync(email);
                if (acc == null)
                {
                    return new ApiResponse { StatusCode = 404, Message = "User not found." };
                }

                if (Encryption.Decrypt(acc.Password) == newPassword)
                {
                    return new ApiResponse { StatusCode = 400, Message = "The new password cannot be the same as the old password." };
                }

                acc.Password = Encryption.Encrypt(newPassword);
                await _account.UpdateAccount(acc);
                return new ApiResponse { StatusCode = 200, Message = "Password updated successfully." };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating password: {ex.Message}");
                return new ApiResponse { StatusCode = 500, Message = $"Error updating password: {ex.Message}" };
            }
        }

    }
}
