using Microsoft.EntityFrameworkCore;
using Repositories.DatabaseConnection;
using Repositories.Interfaces;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Transaction = Repositories.Models.Transaction;

namespace Repositories.Implements
{
        public class TransactionRepository : ITransactionRepository
        {
            private readonly MerakiDbContext _context;

            public TransactionRepository(MerakiDbContext context)
            {
                _context = context;
            }

            public async Task<string> AutoGenerateTransactionId()
            {
                string newTransactionId = "";
                string latestTransactionId = await GetLatestTransactionAsync();
                if (string.IsNullOrEmpty(latestTransactionId))
                {
                    newTransactionId = "TR00000001";
                }
                else
                {
                    int numericpart = int.Parse(latestTransactionId.Substring(2));
                    int newnumericpart = numericpart + 10;
                    newTransactionId = $"TR{newnumericpart:d8}";
                }
                return newTransactionId;
            }

            public async Task<Transaction> CreateTransaction(string orderId,string depositId)
            {
                Transaction transaction = new Transaction()
                {
                    TransactionId = await AutoGenerateTransactionId(),
                    DepositId = depositId,
                    OrderId = orderId,
                    TransactionType = "",
                    Status = "Pending",
                    CreatedAt = DateTime.Now,
                };
                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();
                return transaction;
            }

            public async Task<List<Transaction>> GetAllTransactions()
            {
                return await _context.Transactions.ToListAsync();
            }

            public async Task<List<Transaction>> GetAllTransactionsByOrderId(string orderId)
            {
                return await _context.Transactions
                    .Where(transaction => transaction.OrderId.Equals(orderId))
                    .ToListAsync();
            }

            public async Task<string> GetLatestTransactionAsync()
            {
                try
                {

                    // Fetch the relevant data from the database
                    var transactionIds = await _context.Transactions
                        .Select(u => u.TransactionId)
                        .ToListAsync();

                    // Process the data in memory to extract and order by the numeric part
                    var lastestTransactionId = transactionIds
                        .Select(id => new { TransactionId = id, NumericPart = int.Parse(id.Substring(2)) })
                        .OrderByDescending(u => u.NumericPart)
                        .ThenByDescending(u => u.TransactionId)
                        .Select(u => u.TransactionId)
                        .FirstOrDefault();

                    return lastestTransactionId;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message, e);
                }
            }

            public async Task<Transaction> GetTransactionById(string transactionId)
            {
                return await _context.Transactions
                    .FirstOrDefaultAsync(transaction => transaction.TransactionId.Equals(transactionId));
            }

            public async Task UpdateTransaction(Transaction transaction)
            {
                _context.Attach(transaction).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

        }

    }
