using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Models;
using Repositories.DTO;
using Services.Interfaces;

namespace Meraki_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        // register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAccountAsync([FromForm] RegisterDTO accountDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new { Errors = errors });
            }
            var existingPhone = await _accountService.GetAccountByPhoneAsync(accountDTO.Phone);
            var existingEmail = await _accountService.GetAccountByEmailAsync(accountDTO.Email);
            if (existingEmail != null)
            {
                if (existingPhone != null)
                {
                    return BadRequest(new { Errors = new List<string> { "Email and Phone is already in use." } });
                }
                else return BadRequest(new { Errors = new List<string> { "Email is already in use." } });
            }
            if (existingPhone != null)
            {
                return BadRequest(new { Errors = new List<string> { "Phone is already in use." } });
            }
            var result = await _accountService.RegisterAsACustomerAsync(accountDTO);
            if (result == 1)
            {
                return Ok(new ApiResponse()
                {
                    StatusCode = 200,
                    Message = "Register Successfull!",
                    Data = result.ToString()
                });
            }
            return BadRequest(result);
        }

        [HttpPost("signup-by-google")]
        public async Task<IActionResult> SignUpGoogleAsStudent([FromForm] RegisterByGoogleDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                    return BadRequest(new { Errors = errors });
                }
                var result = await _accountService.SignUpByGoogleAsync(request.FirebaseToken, request.Phone, request.Birthday, request.Address, request.Gender);
                if (result != null && result is AuthResponse)
                {
                    return Ok(new ApiResponse
                    {
                        StatusCode = result.StatusCode,
                        Message = "Sign-up Successfull",
                        Data = new AuthResponse
                        {
                            Email = result.Email,
                            FullName = result.FirstName,
                            Role = result.Role,
                            Token = result.Token,
                            Success = result.Success,
                        }
                    });
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }

        // login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginDTO loginDTO)
        {
            Account acc = await _accountService.CheckLogin(loginDTO.Email, loginDTO.Password);
            if (acc == null)
            {
                return BadRequest(new { message = "Invalid email or password" });
            }
            if (acc.Status == "PendingToConfirm")
            {
                if (acc.Role == "Customer")
                {
                    return BadRequest(new { message = "Please confirm your email before login" });
                }
            }
            else if (acc.Status == "Inactive" || acc.Status == "Banned")
            {
                return BadRequest(new { message = "Your account has been previously banned or inactive. Please contact the administrator to resolve your issue." });
            }
            //else if (acc.IsDeleted == 1)
            //{
            //    return Ok(new { message = "Your account has been previously deleted. Please contact the administrator to resolve your issue." });
            //}
            int role = 1;
            if (acc.Role == "Customer")
            {
                role = 2;
            }
            else if (acc.Role == "Admin")
            {
                role = 1;
            }
            else if (acc.Role == "Staff")
            {
                role = 3;
            }
            var token = _accountService.GenerateJwtToken(loginDTO.Email, role, 43200);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                //Token = token,
                Message = $"Login successfull!",
                Data = new AuthenResponse
                {
                    AccessToken = token,
                }
            });
        }

        [HttpPost("login-by-google")]
        public async Task<IActionResult> LoginByGoogle([FromForm] LoginGoogleDTO loginGoogle)
        {
            try
            {
                var authResponse = await _accountService.GetFirebaseToken(loginGoogle.FirebaseToken);

                if (authResponse.Token == null)
                {
                    // User needs to complete the sign-up process
                    return BadRequest(new
                    {
                        message = "This account doesn't exist. Please register to gain access to our website"
                    });
                }
                var token = authResponse.Token;
                return Ok(new ApiResponse
                {
                    StatusCode = 200,
                    Message = "Login Successful",
                    Data = new AuthenResponse()
                    {
                        AccessToken = token,
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            //return ve giong login 
        }
        [Authorize(Policy = "CustomerOnly")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("view-profile")]
        public async Task<IActionResult> ViewProfileByCustomer(string accessToken)
        {
            Account account = await _accountService.GetUserAccountAsync(accessToken);
            if (accessToken == null)
            {
                return BadRequest("Cannot get access token");
            }
            if (account == null)
            {
                return BadRequest("Cannot find this account");
            }
            Customer customer = await _accountService.GetCustomerAsync(account.AccountId);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "User Profile",
                Data = new AccountProfileResponse
                {
                    Email = account.Email,
                    FullName = account.FullName,
                    UserName = account.UserName,
                    Phone = account.Phone,
                    Birthday = (DateTime)account.Birthday,
                    Address = account.Address,
                    Gender = account.Gender,
                    Avatar = customer.Avatar,
                    CardName = customer.CardName,
                    CardProviderName = customer.CardProviderName,
                    CardNumber = customer.CardNumber,
                    TaxNumber = customer.TaxNumber,
                }
            });
        }

        [Authorize(Policy = "CustomerOnly")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfileByUser(string accessToken, [FromForm] UpdateAccountProfileDTO accountProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _accountService.UpdateAccountProfileByAdminAsync(accessToken, accountProfile);
            if (result)
            {
                return Ok("Update Successful");
            }
            return NotFound("Account not found or update failed");
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO request)
        {
            try
            {
                var result = await _accountService.ForgotPasswordAsync(request.Email);
                if (result)
                {
                    return Ok("Password reset email sent successfully");
                }
                else if (!result)
                {
                    return NotFound(new ApiResponse
                    {
                        StatusCode = 400,
                        Message = "User not found"
                    });
                }
                else
                {
                    return StatusCode(500, "Failed to send password reset email");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Failed to send password reset email");
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _accountService.ResetPasswordAsync(request.Token, request.NewPassword);

                if (result.StatusCode != 200)
                {
                    return BadRequest(new ApiResponse { StatusCode = 400, Message = result.Message });
                }
                return Ok(new ApiResponse { StatusCode = 200, Message = result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }


    }
}
