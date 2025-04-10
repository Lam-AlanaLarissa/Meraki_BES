using Firebase.Auth;
using Microsoft.EntityFrameworkCore;
using Repositories.DatabaseConnection;
using Repositories.Interfaces;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implements
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MerakiDbContext _context;

        public AccountRepository(MerakiDbContext context)
        {
            _context = context;
        }


        public async Task<Account> GetAccountByEmailAsync(string email)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<Account> GetAccountById(string accountId)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId);
        }
        public async Task<Account> GetAccountByPhoneAsync(double phone)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Phone == phone);
        }

        public async Task<Customer> GetCustomerByAccountIdAsync(string accountId)
        {
            return await _context.Customers.Include(c => c.Account)
                .Where(u => u.AccountId == accountId).FirstOrDefaultAsync();
        }
        public async Task<Customer> GetSellerProfileByAccountIdAsync(string accountId)
        {
            return await _context.Customers.Include(u => u.Account).FirstOrDefaultAsync(u => u.AccountId == accountId);
        }
        public async Task<string> GetLatestAccountIdAsync()
        {
            try
            {

                // Fetch the relevant data from the database
                var accountIds = await _context.Accounts
                    .Select(u => u.AccountId)
                    .ToListAsync();

                // Process the data in memory to extract and order by the numeric part
                var latestAccountId = accountIds
                    .Select(id => new { AccountId = id, NumericPart = int.Parse(id.Substring(2)) })
                    .OrderByDescending(u => u.NumericPart)
                    .ThenByDescending(u => u.AccountId)
                    .Select(u => u.AccountId)
                    .FirstOrDefault();

                return latestAccountId;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }


        public async Task<dynamic> CreateAccountAsync(Account acc)
        {
            try
            {
                using (var context = new MerakiDbContext())
                {
                    await context.Accounts.AddAsync(acc);
                    return await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateAccount(Account acc)
        {
            _context.Accounts.Update(acc);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateCustomer(Customer cus)
        {
            _context.Customers.Update(cus);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Customer> GetCustomerByAccountId(string accountId)
        {
            return await _context.Customers.FirstOrDefaultAsync(u => u.AccountId == accountId);
        }

        public async Task<string> GetLatestCustomerIdAsync()
        {
            try
            {
                // Fetch the relevant data from the database
                var CustomerIds = await _context.Customers
                    .Select(u => u.CustomerId)
                    .ToListAsync();

                // Process the data in memory to extract and order by the numeric part
                var latestCustomerId = CustomerIds
                    .Select(id => new { CustomerId = id, NumericPart = int.Parse(id.Substring(1)) })
                    .OrderByDescending(u => u.NumericPart)
                    .ThenByDescending(u => u.CustomerId)
                    .Select(u => u.CustomerId)
                    .FirstOrDefault();

                return latestCustomerId;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        public async Task<dynamic> CreateCustomerAsync(Customer Customer)
        {
            try
            {
                await _context.Customers.AddAsync(Customer);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<dynamic> DeleteCustomerAsync(Customer Customer)
        {
            try
            {
                _context.Customers.Remove(Customer);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// This function get the payment info of seller to show
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<dynamic> GetBankInfoOfSellerAsync(string accountId)
        {
            return await _context.Customers.Where(u => u.AccountId == accountId).Select(u => new
            {
                u.CardName,
                u.CardNumber,
                u.CardProviderName,
                u.TaxNumber
            }).ToListAsync();
        }

        /// <summary>
        /// This function will get a list of available card providers in db to show with Name and Fullname of it 
        /// </summary>
        /// <returns>List Of Card Providers</returns>
        public async Task<dynamic> GetListBankNameAsync()
        {
            return await _context.CardProviders.Select(cp => new
            {
                cp.CardProviderName,
                cp.CpfullName
            }).ToListAsync();
        }

        public async Task<double> GetAccountBalance(string accountId)
        {
            var accountBalance = await _context.CustomerWallets
                .FirstOrDefaultAsync(wallet => wallet.AccountId.Equals(accountId));
            if (accountBalance == null) return 0;
            return (double)accountBalance.Balance;
        }

    }
}
