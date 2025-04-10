using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class CreateProductDTO
    {
        [Required]
        public string PcateId { get; set; } = null!;
        [Required]
        public string ProductName { get; set; } = null!;
        [Required]
        public string ProductDesc { get; set; } = null!;
        [Required]
        public decimal? ProductPrice { get; set; }
        [Required]
        public decimal? Discount { get; set; }
        [Required]
        public string Location { get; set; } = null!;
        [Required]
        public string PurchaseType { get; set; } = null!;
        [Required]
        public DateOnly? PurchaseDate { get; set; }
        [Required]
        public decimal? PercentageOfDamage { get; set; }
        [Required]
        public string? DamageDetail { get; set; }
        [Required]
        public List<IFormFile> Attachments { get; set; }
        [Required]
        public string Status { get; set; } = null!;

    }
}
