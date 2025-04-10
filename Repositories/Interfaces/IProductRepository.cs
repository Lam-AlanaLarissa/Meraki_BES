using Repositories.DTO;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IProductRepository
    {
        public Task<Product> GetProductByProductIdAsync(string ProductId);
        public Task<Product> GetProductByProductNameAsync(string ProductName);
        public Task<string> GetProductNameByProductId(string ProductId);
        public Task<List<Product>> GetListProductByAccountId(string accountId);
        public Task<string> GetLatestProductIdAsync();
        public Task<dynamic> CreateProductAsync(Product newProduct);
        public Task<dynamic> UpdateProductAsync(Product Product);
        public Task<dynamic> DeleteListOfProductsAsEverAsync(List<Product> Products);
        public Task<(List<ProductListDTO> products, int totalCount, int totalPages)> GetListOfActiveProduct(int pageIndex, int pageSize, string sortBy, bool sortDesc, string search);
    }
}
