using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class OrderDetail
    {
        [Key]
        public string OrderDetailId { get; set; } = null!;

        public string OrderId { get; set; }

        [Required]
        public string ProductId { get; set; } = null!;
        

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public double PaidPrice { get; set; }

        public string? OrderNumber { get; set; }

        // Relationships
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }  // Nullable for barter trades

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

    }

}
