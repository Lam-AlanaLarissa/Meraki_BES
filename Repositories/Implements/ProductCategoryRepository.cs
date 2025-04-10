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
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly MerakiDbContext _context;

        public ProductCategoryRepository(MerakiDbContext context)
        {
            _context = context;
        }
        public async Task<ProductCategory> GetProductCategoryByCateIdAsync(string cateId)
        {
            return await _context.ProductCategories.FirstOrDefaultAsync(fc => fc.PcateId == cateId);
        }

        public async Task<List<ProductCategory>> GetProductCategoryListAsync()
        {
            return await _context.ProductCategories.Where(fc => fc.PcateStatus == "Active").ToListAsync();
            // status: 1. Active 2. Inactive
        }
        public async Task<string> GetLatestProductCategoryIdAsync()
        {
            try
            {

                // Fetch the relevant data from the database
                var ProductCategoryIds = await _context.ProductCategories
                    .Select(u => u.PcateId)
                    .ToListAsync();

                // Process the data in memory to extract and order by the numeric part
                var latestProductCategoryId = ProductCategoryIds
                    .Select(id => new { PcateId = id, NumericPart = int.Parse(id.Substring(2)) })
                    .OrderByDescending(u => u.NumericPart)
                    .ThenByDescending(u => u.PcateId)
                    .Select(u => u.PcateId)
                    .FirstOrDefault();

                return latestProductCategoryId;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        public async Task<dynamic> CreateProductCategoryAsync(ProductCategory cate)
        {
            try
            {
                using (var context = new MerakiDbContext())
                {
                    await context.ProductCategories.AddAsync(cate);
                    return await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error at CreateProductCategoryAsync() - repository: + {ex.Message}");
            }
        }

        public async Task<dynamic> UpdateProductCategorygoryAsync(ProductCategory cate)
        {
            try
            {
                _context.ProductCategories.Update(cate);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error at UpdateProductCategorygoryAsync: {ex.Message}");
            }
        }
    }
}
