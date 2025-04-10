using CloudinaryDotNet.Actions;
using Repositories.Interfaces;
using Repositories.Models;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implements
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ISellerRepository _sellerRepository;
        public CartService(ICartRepository cartRepository, IAccountRepository accountRepository, IProductRepository productRepository, IOrderRepository orderRepository, ISellerRepository sellerRepository)
        {
            _cartRepository = cartRepository;
            _accountRepository = accountRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _sellerRepository = sellerRepository;
        }
        public async Task<string> AutoGenerateOrderDetailId()
        {
            string newOrderDetailId = "";
            string latestOrderDetailId = await _cartRepository.GetLatestOrderDetailIdAsync();
            if (string.IsNullOrEmpty(latestOrderDetailId))
            {
                newOrderDetailId = "OD00000001";
            }
            else
            {
                int numericpart = int.Parse(latestOrderDetailId.Substring(2));
                int newnumericpart = numericpart + 1;
                newOrderDetailId = $"OD{newnumericpart:d8}";
            }
            return newOrderDetailId;
        }

        public async Task<dynamic> AddToCartAsync(AddToCartDTO cartDTO)
        {
            var buyerEmail = TokenDecoder.GetEmailFromToken(cartDTO.accessToken);
            var buyer = await _accountRepository.GetAccountByEmailAsync(buyerEmail);
            if (buyer == null)
            {
                return new
                {
                    Message = "Cannot find your account",
                    StatusCode = 404
                };
            }
            var Product = await _productRepository.GetProductByProductIdAsync(cartDTO.ProductID);
            if (Product == null)
            {
                return new
                {
                    Message = "Product is not already exist",
                    StatusCode = 404
                };
            }
            if (Product.AccountId == buyer.AccountId)
            {
                return new
                {
                    Message = "Cannot Buy This Product! Product is your shop's owner"
                };
            }
            if (cartDTO.Quantity > Product.Quantity)
            {
                return new
                {
                    Message = $"Exceed the total number of Products in stock",
                    StatusCode = 409
                };
            }
            string ProductId = Product.ProductId;
            var existProductInCart = await _cartRepository.GetCartItemByProductIdAndAccountAsync(ProductId, buyer.AccountId);

            if (existProductInCart != null)
            {
                existProductInCart.Quantity = existProductInCart.Quantity + cartDTO.Quantity;
                existProductInCart.PaidPrice = Product.Price * existProductInCart.Quantity;
                await _cartRepository.UpdateCartAsync(existProductInCart);
                return new
                {
                    Message = "Add to exist product in cart successful",
                    StatusCode = 201,
                    ExistCart = new
                    {
                        existProductInCart.ProductId,
                        Product.ProductName,
                        existProductInCart.Quantity,
                        existProductInCart.PaidPrice,
                        UnitPrice = Product.Price,
                    }
                };
            }
            else
            {
                //var seller = await _accountRepository.GetUserByAccountIdAsync(Product.AccountId);
                var ordersInCart = await _orderRepository.GetListOrderNotPaymentByAccountIdAsync(buyer.AccountId);

                foreach (var item in ordersInCart)
                {
                    if (item.SellerId.Equals(Product.AccountId))
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderDetailId = await AutoGenerateOrderDetailId(),
                            OrderId = item.OrderId,
                            ProductId = ProductId,
                            Quantity = cartDTO.Quantity,
                            PaidPrice = cartDTO.Quantity * Product.Price,
                            AccountId = buyer.AccountId,
                        };
                        await _cartRepository.AddToCartAsync(orderDetail);
                        return new
                        {
                            Message = "Add To Cart Successful",
                            StatusCode = 201,
                            Item = new
                            {
                                orderDetail.ProductId,
                                Product.ProductName,
                                orderDetail.Quantity,
                                orderDetail.PaidPrice,
                                UnitPrice = Product.Price,
                            }
                        };
                    }
                }
                var order = await _orderRepository.CreateOrder(new Order()
                {
                    OrderId = await _orderRepository.AutoGenerateOrderId(),
                    Account1Id = buyer.AccountId,
                    Account2Id = Product.AccountId,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    Status = 1,
                    PaymentStatus = 0,
                    TotalMoney = 0,
                    Detail = "",
                    FullName = buyer.FullName,
                    Address = buyer.Address,
                    PhoneNumber = buyer.PhoneNumber
                });

                var cartItem = new OrderDetail
                {
                    OrderDetailId = await AutoGenerateOrderDetailId(),
                    OrderId = order.OrderId,
                    ProductId = ProductId,
                    Quantity = cartDTO.Quantity,
                    PaidPrice = cartDTO.Quantity * Product.Price,
                    AccountId = buyer.AccountId,
                };
                await _cartRepository.AddToCartAsync(cartItem);
                return new
                {
                    Message = "Add To Cart Successful",
                    StatusCode = 201,
                    Item = new
                    {
                        cartItem.ProductId,
                        Product.ProductName,
                        cartItem.Quantity,
                        cartItem.PaidPrice,
                        UnitPrice = Product.Price,
                    }
                };
            }


        }
        public async Task<dynamic> DeleteCartItemAsync(string cartItemId)
        {
            var cartItem = await _cartRepository.GetCartByOrderDetailId(cartItemId);
            if (cartItem == null)
            {
                return new
                {
                    Message = "This cart is null",
                    StatusCode = 404
                };
            }

            var orderOfCart = await _orderRepository.GetOrderById(cartItem.OrderId);
            var existCartOfShops = await _cartRepository.GetCartItemsByOrderId(orderOfCart.OrderId);

            dynamic result = null;
            if (existCartOfShops.Count() > 1)
            {
                result = await _cartRepository.DeleteCartAsync(cartItem);
            }
            else if (existCartOfShops.Count() == 1)
            {
                await _cartRepository.DeleteCartAsync(cartItem);
                result = await _orderRepository.DeleteOrder(orderOfCart);
            }
            return result;
        }

        public async Task<dynamic> UpdateCartAsync(string cartItemId, double quantity)
        {
            var existCartItem = await _cartRepository.GetCartItemByCartIdAsync(cartItemId);
            if (existCartItem == null)
            {
                return new
                {
                    Message = "This cart is null",
                    StatusCode = 404
                };
            }
            var ProductInCart = await _productRepository.GetProductByProductIdAsync(existCartItem.ProductId);
            if (ProductInCart == null)
            {
                return new
                {
                    Message = "Product is not already exist",
                    StatusCode = 404
                };
            }

            if (quantity == 0)
            {
                return new
                {
                    Message = "This cart item will be delete",
                    StatusCode = 409
                };
            }
            else if (quantity > existCartItem.Quantity)
            {
                return new
                {
                    Message = $"Exceed the total number of Products in stock",
                    StatusCode = 409
                };
            }
            existCartItem.Quantity = existCartItem.Quantity + quantity;
            existCartItem.PaidPrice = ProductInCart.Price * existCartItem.Quantity;
            var result = await _cartRepository.UpdateCartAsync(existCartItem);
            return new
            {
                Message = "Update To Cart Successful",
                StatusCode = 201,
                Item = new
                {
                    existCartItem.ProductId,
                    existCartItem.Quantity,
                    existCartItem.PaidPrice,
                    ProductInCart.ProductName,
                    UnitPrice = ProductInCart.Price,
                }
            };
        }

        // original
        //public async Task<dynamic> GetCartListAsync(string accessToken)
        //{
        //    string accEmail = TokenDecoder.GetEmailFromToken(accessToken);
        //    var acc = await _accountRepository.GetAccountByEmailAsync(accEmail);
        //    var result = await _cartRepository.GetListCartOfUser(acc.AccountId);
        //    return result;
        //}

        // 1
        //public async Task<dynamic> GetCartListAsync(string accessToken)
        //{
        //    string accEmail = TokenDecoder.GetEmailFromToken(accessToken);
        //    var acc = await _accountRepository.GetAccountByEmailAsync(accEmail);
        //    var listCart = await _cartRepository.GetListCartOfUser(acc.AccountId);
        //    var orders = await _orderRepository.GetListOrderNotPaymentByAccountIdAsync(acc.AccountId);
        //    //List<List<CartListDTO>> list = new()
        //    //{

        //    //};
        //    var groupedOrders = new List<List<CartListDTO>>();
        //    var shopOrderDetailsMap = new Dictionary<string, List<CartListDTO>>(); // Grouping by shop

        //    foreach (var cartItem in orders)
        //    {
        //        var orderDetails = listCart.Where(cart => cart.OrderId == cartItem.OrderId).ToList();
        //        foreach (var detail in orderDetails)
        //        {
        //            var Product = await _productRepository.GetProductByProductIdAsync(detail.ProductId);
        //            if (Product != null)
        //            {
        //                string accId = Product.AccountId;
        //                detail.ProductName = Product.ProductName;
        //                // Group order details by shop (implicitly using AccountId)
        //                if (!shopOrderDetailsMap.ContainsKey(accId)) // Use AccountId as the key
        //                {
        //                    shopOrderDetailsMap[accId] = new List<CartListDTO>();
        //                }
        //                shopOrderDetailsMap[accId].Add(detail); // Add detail to the corresponding shop
        //            }
        //        }
        //    }

        //    // Convert the mapping to a list of lists for the response
        //    groupedOrders = shopOrderDetailsMap.Values.ToList();

        //    return groupedOrders;
        //}


        // 2
        //public async Task<dynamic> GetCartListAsync(string accessToken)
        //{
        //    string accEmail = TokenDecoder.GetEmailFromToken(accessToken);
        //    var acc = await _accountRepository.GetAccountByEmailAsync(accEmail);
        //    var listCart = await _cartRepository.GetListCartOfUser(acc.AccountId);
        //    var orders = await _orderRepository.GetListOrderNotPaymentByAccountIdAsync(acc.AccountId);
        //    List<CartItemViewDTO> list = new()
        //    {
        //    };
        //    foreach (var item in orders)
        //    {
        //        var cartItem = new CartItemViewDTO();
        //        var listDetails = new List<OrderDetailResponse>();
        //        var orderDetails = listCart
        //                        .Where(cart => cart.OrderId == item.OrderId)
        //                        .ToList();
        //        foreach (var detail in orderDetails)
        //        {
        //            listDetails.Add(new OrderDetailResponse()
        //            {
        //                OrderDetailId = detail.OrderDetailId,
        //                OrderId = item.OrderId,
        //                OrderNumber = detail.OrderNumber,
        //                PaidPrice = detail.PaidPrice,
        //                Quantity = detail.Quantity,
        //                Product = await _productRepository.GetProductByProductIdAsync(detail.ProductId)
        //            });
        //        }
        //        cartItem.OrderDetails = listDetails;
        //        cartItem.Seller = await _sellerRepository.GetShopDetailByAccountId(item.SellerId);
        //        list.Add(cartItem);
        //    }
        //    return list;
        //}

        // 3
        public async Task<dynamic> GetCartListAsync(string accessToken)
        {
            string accEmail = TokenDecoder.GetEmailFromToken(accessToken);
            var acc = await _accountRepository.GetAccountByEmailAsync(accEmail);
            var listCart = await _cartRepository.GetListCartOfUser(acc.AccountId);
            var orders = await _orderRepository.GetListOrderNotPaymentByAccountIdAsync(acc.AccountId);
            List<CartItemViewDTO> list = new()
            {
            };
            foreach (var item in orders)
            {
                var cartItem = new CartItemViewDTO();
                var listDetails = new List<OrderDetailResponse>();
                var orderDetails = listCart
                                .Where(cart => cart.OrderId == item.OrderId)
                                .ToList();
                foreach (var detail in orderDetails)
                {
                    listDetails.Add(new OrderDetailResponse()
                    {
                        OrderDetailId = detail.OrderDetailId,
                        OrderId = item.OrderId,
                        OrderNumber = detail.OrderNumber,
                        PaidPrice = detail.PaidPrice,
                        Quantity = detail.Quantity,
                        Product = await _productRepository.GetProductByProductIdAsync(detail.ProductId)
                    });
                }
                cartItem.OrderDetails = listDetails;
                var seller = await _sellerRepository.GetShopDetailByAccountId(accountId: item.SellerId);
                cartItem.Seller = new SellerInCartDTO()
                {
                    SellerAvatar = seller.SellerAvatar,
                    ShopName = seller.ShopName
                };
                list.Add(cartItem);
            }
            return list;
        }
    }
}
