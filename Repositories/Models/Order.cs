using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class Order
    {
        [Key]
        public string OrderId { get; set; } = null!;

        public string Detail { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }  // Changed from DateOnly for better compatibility

        [Required]
        public string Account1Id { get; set; } = null!; // Buyer/User initiating the order

        public string? Account2Id { get; set; }  // Nullable: Exists only for Buy transactions

        [Required]
        public string OrderType { get; set; } = "Buy";  // "Buy" or "Barter"

        public long Status { get; set; } = 0;  // Default status

        [Required]
        public double TotalMoney { get; set; }

        [Required]
        public int PaymentStatus { get; set; }  // 0 = Unpaid, 1 = Paid

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        // Relationships
        [ForeignKey("Account1Id")]
        public virtual Account Account1 { get; set; } = null!;  // Buyer

        [ForeignKey("Account2Id")]
        public virtual Account? Account2 { get; set; }  // Optional seller for purchases

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
