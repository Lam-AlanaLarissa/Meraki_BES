using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class OrderListDTO
    {
        public string OrderId { get; set; } = null!;

        public string Detail { get; set; }

        public DateTime OrderDate { get; set; }  // Changed from DateOnly for better compatibility

        public string AccountName1 { get; set; } = null!; // Buyer/User initiating the order

        public string? AccountName2 { get; set; }  // Nullable: Exists only for Buy transactions

        public string OrderType { get; set; } = "Buy";  // "Buy" or "Barter"

        public long Status { get; set; } = 0;  // Default status

        public double TotalMoney { get; set; }

        public int PaymentStatus { get; set; }  // 0 = Unpaid, 1 = Paid

        public string FullName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; }
    }
}
