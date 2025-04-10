using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Repositories.DatabaseConnection;
using Repositories.DTO;
using Repositories.Interfaces;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implements
{
    public class ProductRepository : IProductRepository
    {
        private readonly MerakiDbContext _context;

        public ProductRepository(MerakiDbContext context)
        {
            _context = context;
        }

        // Get Product by attribute
        // Get Product by attribute
        public async Task<Product> GetProductByProductIdAsync(string ProductId)
        {
            return await _context.Products.FirstOrDefaultAsync(f => f.ProductId == ProductId);
        }
        public async Task<Product> GetProductByProductNameAsync(string ProductName)
        {
            return await _context.Products.FirstOrDefaultAsync(f => f.ProductId.Equals(ProductName));
        }
        public async Task<string> GetProductNameByProductId(string ProductId)
        {
            return await _context.Products
                     .Where(f => f.ProductId == ProductId)
                     .Select(f => f.ProductName)
                     .FirstOrDefaultAsync();
        }
        public async Task<List<Product>> GetListProductByAccountId(string accountId)
        {
            return await _context.Products.Where(f => f.AccountId == accountId).ToListAsync();
        }

        // Get the latest Product id
        public async Task<string> GetLatestProductIdAsync()
        {
            try
            {

                // Fetch the relevant data from the database
                var ProductIds = await _context.Products
                    .Select(u => u.ProductId)
                    .ToListAsync();

                // Process the data in memory to extract and order by the numeric part
                var latestProductId = ProductIds
                    .Select(id => new { ProductId = id, NumericPart = int.Parse(id.Substring(1)) })
                    .OrderByDescending(u => u.NumericPart)
                    .ThenByDescending(u => u.ProductId)
                    .Select(u => u.ProductId)
                    .FirstOrDefault();

                return latestProductId;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        // for crud product
        public async Task<dynamic> CreateProductAsync(Product newProduct)
        {
            try
            {
                using (var context = new MerakiDbContext())
                {
                    await context.Products.AddAsync(newProduct);
                    return await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error at CreateProductAsync() of ProductRepository + {ex}");
            }
        }

        public async Task<dynamic> UpdateProductAsync(Product Product)
        {
            try
            {
                _context.Products.Update(Product);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error at UpdateProductAsync() in Repository: {ex.Message}");
            }
        }

        public async Task<dynamic> DeleteListOfProductsAsEverAsync(List<Product> Products)
        {
            _context.Products.RemoveRange(Products);
            return await _context.SaveChangesAsync() > 0;
        }



        public async Task<(List<ProductListDTO> products, int totalCount, int totalPages)> GetListOfActiveProduct(int pageIndex, int pageSize, string sortBy, bool sortDesc, string search)
        {
            var query = _context.Products.Include(p => p.Pcate)
                        .Include(p => p.Account)
                        .Where(p => p.Status == "Active").AsQueryable();
            //Search
            if (!string.IsNullOrEmpty(search))
            {
                int.TryParse(search, out int searchId);
                query = query.Where(i => i.ProductName.Contains(search));
            }

            //if (!string.IsNullOrEmpty(sortBy))
            //{
            //    var sortDirection = sortDesc ? "descending" : "ascending";
            //    var sortExpression = $"{sortBy} {sortDirection}";
            //    query = query.OrderBy(sortExpression);
            //}
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = ApplySorting(query, sortBy, sortDesc);
            }

            // Total count before paging
            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // Paging
            var products = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(c => new ProductListDTO
            {
                ProductId = c.ProductId,
                ProductName = c.ProductName,
                PCateName = c.Pcate.PcateName,
                ProductDesc = c.ProductDesc,
                Discount = c.Discount,
                Location = c.Location,
                ProductPrice = c.ProductPrice,
                PurchaseDate = c.PurchaseDate,
                PurchaseType = c.PurchaseType,
                PercentageOfDamage = c.PercentageOfDamage,
                DamageDetail = c.DamageDetail,
                Status = c.Status,
                Images = c.Images,
                CreatedAt = c.CreatedAt,
                CreatedBy = c.CreatedBy,
                UpdatedAt = c.UpdatedAt,
                UpdatedBy = c.UpdatedBy,
                DeletedAt = c.DeletedAt,
                DeletedBy = c.DeletedBy,
                CustomerName = _context.Accounts.Where(u => u.AccountId == c.AccountId)
                                  .Select(u => u.FullName).FirstOrDefault(),

            }).ToList();

            return (products, totalCount, totalPages);
        }

        private IQueryable<Product> ApplySorting(IQueryable<Product> query, string sortBy, bool sortDesc)
        {
            query = sortBy.ToLower() switch
            {
                "productName" => sortDesc ? query.OrderByDescending(p => p.ProductName) : query.OrderBy(p => p.ProductName),
                "cateId" => sortDesc ? query.OrderByDescending(p => p.Pcate.PcateName) : query.OrderBy(p => p.Pcate.PcateName),
                _ => query // Default case if no valid sorting field is provided

            };
            return query;
        }
    }
}
