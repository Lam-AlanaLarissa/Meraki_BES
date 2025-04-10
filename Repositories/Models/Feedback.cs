using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class Feedback
    {
        [Key] public string FeedbackId { get; set; }
        public string Detail { get; set; }
        public double Rating { get; set; }
        public string Attachment { get; set; }
        public DateTime CreatedDate { get; set; }
        public string IsDeleted { get; set; }
        [ForeignKey("ProductId")] public string ProductID { get; set; }
        public virtual Product Product { get; set; }
        public bool IsGoodReview { get; set; } = false;
        [ForeignKey("AccountId")] public string AccountId { get; set; }// WHO SENDS FEEDBACK
        public virtual Account Account { get; set; }

    }
}
