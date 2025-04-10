using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class FeedbackDTO
    {
        public string FeedbackId { get; set; } = null!;

        public string Detail { get; set; } = null!;

        public double Rating { get; set; }

        public string ProductId { get; set; } = null!;
        public string AccountId { get; set; } = null!;
        public string AccountName { get; set; } = null!;

        public DateTime CreateDate { get; set; }

        public bool IsGoodReview { get; set; }

    }
}
