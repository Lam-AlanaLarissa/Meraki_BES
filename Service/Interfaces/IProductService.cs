using Repositories.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IProductService
    {
        public Task<dynamic> CreateNewProductAsync(string accessToken, CreateProductDTO newProduct);
        public Task<dynamic> UpdateProduct(string accessToken, UpdateProductDTO updateProduct);
        public Task<dynamic> DeleteAProductAsync(string accessToken, List<string> productIds);
        public Task<dynamic> InactiveAndActiveProductByOwner(string accessToken, string productId);
        public Task<dynamic> ViewProductDetailAsync(string productId);
        public Task<(List<ProductListDTO> products, int totalCount, int totalPages)> GetListOfActiveProductAsync(int pageIndex, int pageSize, string sortBy, bool sortDesc, string search);
    }
}
