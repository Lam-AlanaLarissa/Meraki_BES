using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class UpdateProductDTO
    {
        [Required]
        [RegularExpression(@"^[A-Z]{2}\d{8}$", ErrorMessage = "CateId must be in the format of two uppercase letters followed by eight digits (e.g., PC00000001).")]
        public string PcateId { get; set; } = null!;
        [Required]
        public string ProductName { get; set; } = null!;
        [Required]
        public string ProductDesc { get; set; } = null!;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal? ProductPrice { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Discount cannot be negative.")]
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
