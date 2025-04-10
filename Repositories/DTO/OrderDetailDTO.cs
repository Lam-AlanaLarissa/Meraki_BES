using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class OrderDetailDTO
    {
        public string OrderDetailId { get; set; } = null!;

        public string OrderId { get; set; }

        public string ProductName { get; set; } = null!;

        public string ProductImage { get; set; } = null!;
        public decimal Quantity { get; set; }


        public double PaidPrice { get; set; }

        public string? OrderNumber { get; set; }
        public string? AccountId { get; set; }
    }
}
