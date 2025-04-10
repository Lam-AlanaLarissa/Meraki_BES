using Repositories.DTO;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAccountService
    {
        public Task<dynamic> RegisterAsACustomerAsync(RegisterDTO accountDTO);
        public Task<Account> GetAccountByEmailAsync(string email);
        public Task<Account> GetAccountByPhoneAsync(double phone);
        public Task<Account> CheckLogin(string email, string password);
        public Task<dynamic> SignUpByGoogleAsync(string firebaseToken, long phone, DateTime birthday, string address, string gender);
        public Task<dynamic> GetUserAccountAsync(string accessToken);
        public Task<dynamic> GetCustomerAsync(string accessToken);
        public Task<AuthResponse> GetFirebaseToken(string firebaseToken); public string GenerateJwtToken(string email, int Role, double expirationMinutes);
        public Task<bool> UpdateAccountProfileByAdminAsync(string accessToken, UpdateAccountProfileDTO accountProfile);

        public Task<bool> ForgotPasswordAsync(string email);
        public Task<ApiResponse> ResetPasswordAsync(string token, string newPassword);

    }
}
