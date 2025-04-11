using Repositories.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICartService
    {
        public Task<dynamic> AddToCartAsync(AddToCartDTO cartDTO);
        public Task<dynamic> UpdateCartItemAsync(string cartItemId, double? quantity);
        public Task<dynamic> DeleteCartItemAsync(string cartItemId);
        public Task<dynamic> GetCartListAsync();
    }
}
