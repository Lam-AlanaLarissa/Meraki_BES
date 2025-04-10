using Microsoft.EntityFrameworkCore;
using Repositories.DatabaseConnection;
using Repositories.DTO;
using Repositories.Interfaces;
using Repositories.Models;
using Repositories.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implements
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly MerakiDbContext _context;

        public FeedbackRepository(MerakiDbContext context)
        {
            _context = context;
        }

        public async Task<Feedback> GetFeedbackByIdAsync(string feedbackId)
        {
            return await _context.Feedbacks.FirstOrDefaultAsync(fb => fb.FeedbackId == feedbackId);
        }

        public async Task<List<Feedback>> GetListOfFeedbackAsync()
        {
            return await _context.Feedbacks.Select(fb => new Feedback
            {
                FeedbackId = fb.FeedbackId,
                Detail = fb.Detail,
                Rating = fb.Rating,
                ProductID = fb.ProductID,
                AccountId = fb.AccountId,
                CreatedDate = fb.CreatedDate,
                IsGoodReview = fb.IsGoodReview,
            }).ToListAsync();
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
        public async Task<string> GetLatestFeedbackIdAsync()
        {
            try
            {

                // Fetch the relevant data from the database
                var feedbackIds = await _context.Feedbacks
                    .Select(u => u.FeedbackId)
                    .ToListAsync();

                // Process the data in memory to extract and order by the numeric part
                var latestFeedbackId = feedbackIds
                    .Select(id => new { FeedbackId = id, NumericPart = int.Parse(id.Substring(2)) })
                    .OrderByDescending(u => u.NumericPart)
                    .ThenByDescending(u => u.FeedbackId)
                    .Select(u => u.FeedbackId)
                    .FirstOrDefault();

                return latestFeedbackId;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        // CRUD Feedback
        public async Task<dynamic> AddFeedbackAsync(Feedback feedback)
        {
            await _context.Feedbacks.AddAsync(feedback);
            return await _context.SaveChangesAsync();
        }

        public async Task<dynamic> UpdateFeedbackAsync(Feedback feedback)
        {
            _context.Feedbacks.Update(feedback);
            return await _context.SaveChangesAsync();
        }

        // Get List of Feedback
        public async Task<(List<FeedbackListDTO> Feebacks, int TotalCount, int TotalPages)> GetListFeedbackOfProduct(int pageIndex, int pageSize, string sortBy, bool sortDesc, string search)
        {
            var ProductName = search;
            var query = _context.Feedbacks.Include(fb => fb.Product).Include(fb => fb.Account).Where(fb => fb.Product.ProductName == ProductName).AsQueryable();
            // search
            if (!string.IsNullOrEmpty(search))
            {
                int.TryParse(search, out int searchId);
                query = query.Where(fb => fb.Product.ProductName.Contains(ProductName));
            }
            if (!string.IsNullOrEmpty(sortBy))
            {
                var sortDirection = sortDesc ? "descending" : "ascending";
                var sortExpression = $"{sortBy} {sortDirection}";
                query = ApplySortingForFeedback(query, sortBy, sortDesc);
            }

            // Total count before paging
            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var feedbacks = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(fb => new FeedbackListDTO
            {
                FeedbackId = fb.FeedbackId,
                Detail = fb.Detail,
                UserName = fb.Account.UserName,
                Rating = fb.Rating,
                CreatedDate = fb.CreatedDate,
                IsGoodReview = fb.IsGoodReview,
            }).ToListAsync();
            return (feedbacks, totalCount, totalPages);
        }
        private IQueryable<Feedback> ApplySortingForFeedback(IQueryable<Feedback> query, string sortBy, bool sortDesc)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return query;
            }

            switch (sortBy.ToLower())
            {
                case "username":
                    query = sortDesc ? query.OrderByDescending(c => c.Account.UserName) : query.OrderBy(c => c.Account.UserName);
                    break;
                case "rating":
                    query = sortDesc ? query.OrderByDescending(c => c.Rating) : query.OrderBy(c => c.Rating);
                    break;
                case "detail":
                    query = sortDesc ? query.OrderByDescending(c => c.Detail) : query.OrderBy(c => c.Detail);
                    break;
                default:
                    query = query.OrderBy(c => c.ProductID); // Default sorting
                    break;
            }

            return query;
        }

        public async Task<(List<FeedbackListDTO> Feedbacks, int TotalCount)> GetFeedbackbyProductIdAsync(string productId, int pageIndex, int pageSize, string sortBy, bool sortDesc, string search)
        {
            var query = _context.Feedbacks
                .Include(fb => fb.Account)
                .Include(fb => fb.Account)
                .Where(c => c.ProductID == productId)
                .AsQueryable();

            // Search
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.ProductID.Contains(search));
            }

            query = ApplySortingForFeedback(query, sortBy, sortDesc);
            var totalCount = await query.CountAsync();
            var feedbacks = await query.Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
                                        .Select(c => new FeedbackListDTO
                                        {
                                            FeedbackId = c.FeedbackId,
                                            IsGoodReview = c.IsGoodReview,
                                            ProductId = c.ProductID,
                                            UserName = c.Account.UserName,
                                            CreatedDate = c.CreatedDate,
                                            Rating = c.Rating,
                                            Detail = c.Detail,
                                        })
                                        .ToListAsync();

            return (feedbacks, totalCount);
        }


    }
}
