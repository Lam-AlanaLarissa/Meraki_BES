using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class FeedbackListDTO
    {
        public string FeedbackId { get; set; }
        public bool IsGoodReview { get; set; }
        public string ProductId { get; set; }
        public string UserName { get; set; }
        public string Detail { get; set; }
        public double Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        public IFormFile? Attachments { get; set; }
    }
}
