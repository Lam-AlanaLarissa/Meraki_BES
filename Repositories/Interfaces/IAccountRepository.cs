using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IAccountRepository
    {
        public Task<Account> GetAccountByEmailAsync(string email);
        public Task<Account> GetAccountById(string accountId);
        public Task<Account> GetAccountByPhoneAsync(double phone);
        public Task<Customer> GetCustomerByAccountIdAsync(string accountId);
        public Task<Customer> GetSellerProfileByAccountIdAsync(string accountId);
        public Task<string> GetLatestAccountIdAsync();
        public Task<dynamic> CreateAccountAsync(Account acc);
        public Task<bool> UpdateAccount(Account acc);
        public Task<bool> UpdateCustomer(Customer cus);
        public Task<Customer> GetCustomerByAccountId(string accountId);
        public Task<string> GetLatestCustomerIdAsync();
        public Task<dynamic> CreateCustomerAsync(Customer Customer);
    }
}
