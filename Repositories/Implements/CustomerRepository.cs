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
    public class CustomerRepository : ICustomerRepository
    {
        private readonly MerakiDbContext _context;

        public CustomerRepository(MerakiDbContext context)
        {
            _context = context;
        }
        public async Task<List<Customer>> GetAllCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetShopDetailByAccountId(string accountId)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(item => item.AccountId.Equals(accountId));
        }
    }
}
