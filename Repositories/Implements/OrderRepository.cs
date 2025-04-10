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
    public class OrderRepository : IOrderRepository
    {
        private readonly MerakiDbContext _context;

        public OrderRepository(MerakiDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderByAccountId(string accountId)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.Account1Id == accountId || o.Account2Id == accountId);
        }
        // for order
        public async Task<string> GetLatestOrderIdAsync()
        {
            try
            {

                // Fetch the relevant data from the database
                var orderIds = await _context.Orders
                    .Select(u => u.OrderId)
                    .ToListAsync();

                // Process the data in memory to extract and order by the numeric part
                var latestOrderId = orderIds
                    .Select(id => new { OrderId = id, NumericPart = int.Parse(id.Substring(1)) })
                    .OrderByDescending(u => u.NumericPart)
                    .ThenByDescending(u => u.OrderId)
                    .Select(u => u.OrderId)
                    .FirstOrDefault();

                return latestOrderId;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        public async Task<Order> CreateOrder(Order order)
        {
            try
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                return order;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error at OrderRepository: {ex.InnerException}");
            }
        }

        public async Task<dynamic> UpdateOrder(Order order)
        {
            try
            {
                _context.Orders.Update(order);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error at OrderRepository: {ex.Message}");
            }
        }

        public async Task<dynamic> DeleteOrder(Order order)
        {
            _context.Orders.Remove(order);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<string> AutoGenerateOrderId()
        {
            string newOrderId = "";
            string latestOrderId = await GetLatestOrderIdAsync();
            if (string.IsNullOrEmpty(latestOrderId))
            {
                newOrderId = "O000000001";
            }
            else
            {
                int numericpart = int.Parse(latestOrderId.Substring(1));
                int newnumericpart = numericpart + 1;
                newOrderId = $"O{newnumericpart:d9}";
            }
            return newOrderId;
        }

        // for cart
        public async Task<List<Order>> GetListOrderNotPaymentByAccountIdAsync(string accountId)
        {
            var listOrder = await _context.Orders
                .Where(order => order.Account1Id.Equals(accountId) || order.Account2Id.Equals(accountId) &&
                                order.Status == 1 &&
                                order.PaymentStatus == 0)
                .ToListAsync();
            return listOrder;
        }

        public async Task<double> GetTotalMoneyOfOrder(string orderId)
        {
            var orderDetails = await _context.OrderDetails
                                            .Where(item => item.OrderId.Equals(orderId))
                                            .ToListAsync();
            double totalMoney = (double)0;
            foreach (var item in orderDetails)
            {
                var product1 = await _context.Products
                                    .FirstOrDefaultAsync(p => p.ProductId.Equals(item.ProductId));

                totalMoney += (double)(product1.ProductPrice * item.Quantity);
                var order = await _context.Orders
.FirstOrDefaultAsync(order => order.OrderId.Equals(orderId));
                order.TotalMoney = totalMoney;
                _context.Attach(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            return totalMoney;
        }

        public async Task<Order> GetOrderById(string orderId)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(item => item.OrderId.Equals(orderId)) ?? new Order();
        }

        //public async Task<Order> GetOrderBySellerIdAsync(string? accountId)
        //{
        //    return await _context.Orders.FirstOrDefaultAsync(o => o.SellerId == accountId);
        //}

        public async Task<List<Order>> GetAllOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<int> GetNumberOfOrders()
        {
            return await _context.Orders.CountAsync();
        }

        public async Task<int> GetNumberOfOrderBasedOnStatus(int status)
        {
            return await _context.Orders.CountAsync(item => item.Status == status);
        }
        public async Task<dynamic> GetNumberOrderOfSellerByStatus(string accountId, int status)
        {
            return await _context.Orders.CountAsync(item => item.Account1Id == accountId || item.Account2Id == accountId && item.Status == status);
        }
        public async Task<dynamic> GetNumberOrderOfCustomer(string accountId)
        {
            return await _context.Orders.CountAsync(item => item.Account1Id == accountId || item.Account2Id == accountId);
        }
        public async Task<double> GetEarningOnAllOrders(string accountId)
        {
            var orders = await _context.Orders
                .Where(item => item.Account1Id.Equals(accountId) || item.Account2Id.Equals(accountId) &&
                                item.Status >= 4).ToListAsync();
            var totalEarnings = (double)0;
            foreach (var order in orders)
            {
                totalEarnings += order.TotalMoney;
            }
            return totalEarnings;
        }
        public async Task<dynamic> GetOrderDetailsOfCustomerr(string accountId)
        {
            return await _context.Orders.Include(o => o.OrderDetails).Include(o => o.Account1).Include(o => o.Account2).Where(o => o.Account1Id == accountId || o.Account2Id == accountId).Select(o => new OrderListDTO
            {
                OrderId = o.OrderId,
                Detail = o.Detail,
                OrderDate = o.OrderDate,
                AccountName1 = o.Account1.FullName,
                AccountName2 = o.Account2.FullName,
                Status = o.Status,
                TotalMoney = o.TotalMoney,
                PaymentStatus = o.PaymentStatus,
                FullName = o.FullName,
                Address = o.Address,
                PhoneNumber = o.PhoneNumber,
                OrderDetails = o.OrderDetails
                .Where(od => od.OrderId == o.OrderId) // Filter by OrderId of the current order
              .Join(_context.Products,
                      od => od.ProductId,
                      f => f.ProductId,
                      (od, f) => new OrderDetailDTO
                      {
                          OrderDetailId = od.OrderDetailId,
                          OrderId = od.OrderId,
                          ProductName = od.Product.ProductName,
                          Quantity = od.Quantity,
                          PaidPrice = od.PaidPrice,
                          ProductImage = od.Product.Images,
                          OrderNumber = od.OrderNumber,
                          AccountId = od.Product.AccountId,
                      })
                .ToList()
            }).ToListAsync();
        }



    }
}
