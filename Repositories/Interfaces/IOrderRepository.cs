using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public Task<Order> GetOrderByAccountId(string accountId);
        public Task<string> GetLatestOrderIdAsync();
        public Task<string> AutoGenerateOrderId();
        public Task<Order> CreateOrder(Order order);
        public Task<dynamic> UpdateOrder(Order order);
        public Task<dynamic> DeleteOrder(Order order);

        public Task<List<Order>> GetListOrderNotPaymentByAccountIdAsync(string accountId);
        public Task<double> GetTotalMoneyOfOrder(string orderId);

        public Task<Order> GetOrderById(string orderId);
        public Task<Order> GetOrderByAccountIdAsync(string? accountId);
        public Task<List<Order>> GetAllOrders();
        public Task<int> GetNumberOfOrders();
        public Task<int> GetNumberOfOrderBasedOnStatus(int status);
        public Task<dynamic> GetNumberOrderOfCustomerByStatus(string accountId, int status);

        public Task<dynamic> GetNumberOrderOfCustomer(string accessToken);


        public Task<double> GetEarningOnAllOrders(string accountId);


        // OrderDetail
        public Task<OrderDetail> GetOrderDetailById(string orderDetailId);
        public Task<List<OrderDetail>> GetOrderDetailsByOrderId(string orderId);
        public Task<List<OrderDetail>> GetOrderDetailsByProductId(string productId);
        public Task<string> GetLatestOrderDetailIdAsync();
        public Task<dynamic> GetOrderDetailsOfCustomer(string accountId);
        public Task<string> AutoGenerateOrderDetailId();
        public Task<OrderDetail> CreateOrderDetail(OrderDetail orderDetail);
        public Task<dynamic> UpdateOrderDetail(OrderDetail orderDetail);
        public Task<dynamic> DeleteOrderDetail(OrderDetail orderDetail);
        public Task<OrderDetail> GetCartItemByProductIdAndAccountAsync(string accountId, string productId);

    }
}
