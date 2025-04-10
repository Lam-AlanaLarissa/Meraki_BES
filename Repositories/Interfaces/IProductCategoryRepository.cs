using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IProductCategoryRepository
    {
        public Task<ProductCategory> GetProductCategoryByCateIdAsync(string cateId);
        public Task<List<ProductCategory>> GetProductCategoryListAsync();
        public Task<string> GetLatestProductCategoryIdAsync();
        public Task<dynamic> CreateProductCategoryAsync(ProductCategory cate);
        public Task<dynamic> UpdateProductCategorygoryAsync(ProductCategory cate);

    }
}
