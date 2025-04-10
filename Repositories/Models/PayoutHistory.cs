using Firebase.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public partial class PayoutHistory
    {
        [Key] public string PayoutId { get; set; } = null!;

        public double Amount { get; set; }

        public DateOnly PayoutDate { get; set; }

        public int Status { get; set; }

        [ForeignKey("AccountId")]public string AccountId { get; set; } = null!;

        public virtual Account Account { get; set; } = null!;
    }
}
