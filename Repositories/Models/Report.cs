using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class Report
    {
        [Key] public string ReportId { get; set; }
        public string Issue { get; set; }
        public string? Content { get; set; }
        public string? Attachment { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey("productId")] public string ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
