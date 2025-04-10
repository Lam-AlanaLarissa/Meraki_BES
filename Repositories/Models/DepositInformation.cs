using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class DepositInformation
    {
        [Key]
        public string DepositId { get; set; }
        public string TransactionId { get; set; }
        public string UserId { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("TransactionId")]
        public virtual Transaction Transaction { get; set; }
        [ForeignKey("UserId")]
        public Account User { get; set; }
    }
}
