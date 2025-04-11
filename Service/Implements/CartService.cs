using Microsoft.AspNetCore.Http;
using Repositories.DTO;
using Repositories.Implements;
using Repositories.Interfaces;
using Repositories.Models;
using Services.Common;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implements
{
    public class CartService : ICartService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartService(IAccountRepository accountRepository, IProductRepository productRepository, IOrderRepository orderRepository, IHttpContextAccessor httpContextAccessor)
        {
            _accountRepository = accountRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<dynamic> AddToCartAsync(AddToCartDTO cartDTO)
        {
            var customerEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            var customer = await _accountRepository.GetAccountByEmailAsync(customerEmail);
            if (customer == null)
            {
                return new ApiResponse
                {
                    Message = "Cannot find your account",
                    StatusCode = 404
                };
            }

            var product = await _productRepository.GetProductByProductIdAsync(cartDTO.productId);
            if (product == null)
            {
                return new ApiResponse
                {
                    Message = "Product is not already exist",
                    StatusCode = 404
                };
            }

            if (cartDTO.quantity > product.StockQuantity)
            {
                return new ApiResponse
                {
                    Message = $"Exceed the total number of Products in stock. Only {product.StockQuantity} product(s) in stock.",
                    StatusCode = 400
                };
            }

            if (customer.AccountId == product.AccountId)
            {
                return new ApiResponse
                {
                    Message = "Cannot buy or exchange this product because you are its owner.",
                    StatusCode = 403
                };
            }

            var existingProductInCart = await _orderRepository.GetCartItemByProductIdAndAccountAsync(customer.AccountId, cartDTO.productId);
            if (existingProductInCart != null)
            {
                existingProductInCart.Quantity += cartDTO.quantity;
                existingProductInCart.PaidPrice = (double)product.ProductPrice * existingProductInCart.Quantity;
                await _orderRepository.UpdateOrderDetail(existingProductInCart);
                return new ApiResponse
                {
                    Message = "Product quantity updated in cart successfully.",
                    StatusCode = 200,
                    Data = new
                    {
                        existingProductInCart.ProductId,
                        existingProductInCart.Product.ProductName,
                        existingProductInCart.Quantity,
                        existingProductInCart.PaidPrice,
                        UnitPrice = product.ProductPrice
                    }
                };
            }
            else
            {
                //var order = await _orderRepository.CreateOrder(new Order()
                //{
                //    OrderId = await _orderRepository.AutoGenerateOrderId(),
                //    Account1Id = customer.AccountId,
                //    Account2Id = product.AccountId,
                //    OrderDate = DateTime.UtcNow,
                //    Status = 0,
                //    PaymentStatus = 0,
                //    TotalMoney = 0,
                //    Detail = "",
                //    FullName = customer.FullName,
                //    Address = customer.Address,
                //    PhoneNumber = customer.PhoneNumber
                //});
                var cartItem = new OrderDetail
                {
                    OrderDetailId = await _orderRepository.AutoGenerateOrderDetailId(),
                    OrderId = "IN-CART",
                    ProductId = product.ProductId,
                    Quantity = cartDTO.quantity,
                    PaidPrice = cartDTO.quantity * (double)product.ProductPrice,
                    OrderNumber = "EMPTY",
                    AccountId = customer.AccountId
                };
                await _orderRepository.CreateOrderDetail(cartItem);
                return new ApiResponse
                {
                    Message = "Product added to cart successfully.",
                    StatusCode = 201,
                    Data = new
                    {
                        cartItem.ProductId,
                        product.ProductName,
                        cartItem.Quantity,
                        cartItem.PaidPrice,
                        UnitPrice = product.ProductPrice
                    }
                };
            }
        }

        public async Task<dynamic> UpdateCartItemAsync(string cartItemId, double? quantity)
        {
            var customerEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            var customer = await _accountRepository.GetAccountByEmailAsync(customerEmail);
            if (customer == null)
            {
                return new ApiResponse
                {
                    Message = "Cannot find your account",
                    StatusCode = 404
                };
            }
            var cartItem = await _orderRepository.GetOrderDetailById(cartItemId);
            if (cartItem == null)
            {
                return new ApiResponse
                {
                    Message = "This cart item does not exist",
                    StatusCode = 404
                };
            }
            if (cartItem.AccountId != customer.AccountId)
            {
                return new ApiResponse
                {
                    Message = "You are not authorized to access this cart item.",
                    StatusCode = 403
                };
            }
            var product = await _productRepository.GetProductByProductIdAsync(cartItem.ProductId);
            if (product == null)
            {
                return new ApiResponse
                {
                    Message = "Product is not already exist",
                    StatusCode = 404
                };
            }
            if (quantity == 0)
            {
                return new ApiResponse
                {
                    Message = "This cart item will be deleted",
                    StatusCode = 200
                };
            }
            else if (quantity > product.StockQuantity)
            {
                return new ApiResponse
                {
                    Message = $"Exceed the total number of Products in stock. Only {product.StockQuantity} product(s) in stock.",
                    StatusCode = 400
                };
            }
            cartItem.Quantity = quantity;
            cartItem.PaidPrice = (double)product.ProductPrice * quantity;
            var result = await _orderRepository.UpdateOrderDetail(cartItem);
            return new ApiResponse
            {
                Message = "Cart item updated successfully.",
                StatusCode = 200,
                Data = new
                {
                    cartItem.ProductId,
                    cartItem.Quantity,
                    cartItem.PaidPrice,
                    product.ProductName,
                    UnitPrice = product.ProductPrice
                }
            };
        }
        public async Task<dynamic> DeleteCartItemAsync(string cartItemId)
        {
            var customerEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            var customer = await _accountRepository.GetAccountByEmailAsync(customerEmail);
            if (customer == null)
            {
                return new ApiResponse
                {
                    Message = "Cannot find your account",
                    StatusCode = 404
                };
            }
            var cartItem = await _orderRepository.GetOrderDetailById(cartItemId);
            if (cartItem == null)
            {
                return new ApiResponse
                {
                    Message = "This cart item does not exist",
                    StatusCode = 404
                };
            }
            if (cartItem.AccountId != customer.AccountId)
            {
                return new ApiResponse
                {
                    Message = "You are not authorized to access this cart item.",
                    StatusCode = 403
                };
            }
            var result = await _orderRepository.DeleteOrderDetail(cartItem);
            if (result == 0)
            {
                return new ApiResponse
                {
                    Message = "Failed to delete cart item.",
                    StatusCode = 500
                };
            }

            return new ApiResponse
            {
                Message = "Cart item deleted successfully.",
                StatusCode = 200
            };
        }
        public async Task<dynamic> GetCartListAsync()
        {
            var customerEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            var customer = await _accountRepository.GetAccountByEmailAsync(customerEmail);
            if (customer == null)
            {
                return new ApiResponse
                {
                    Message = "Cannot find your account",
                    StatusCode = 404
                };
            }
            var acc = await _accountRepository.GetAccountByEmailAsync(customerEmail);
            var result = await _orderRepository.GetOrderDetailsOfCustomer(acc.AccountId);
            return result;
        }
    }
}



