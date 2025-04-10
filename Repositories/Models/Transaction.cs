using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class Transaction
    {
        [Key]
        public string TransactionId { get; set; } = Guid.NewGuid().ToString(); // Tạo ID tự động

        public string? DepositId { get; set; }

        public double TotalMoney {  get; set; }
        public string TransactionType { get; set; } = "Buy";
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Trạng thái mặc định là "Pending"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Lưu theo UTC

        public DateTime? CompletedAt { get; set; } // Ngày hoàn tất (nếu có)

        // 🛠 Khóa ngoại
        public string OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [ForeignKey("DepositId")]
        public virtual DepositInformation? DepositInformation { get; set; }
    }
}
