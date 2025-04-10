using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class DetailProductDTO
    {
        public string ProductId { get; set; } = null!;

        public string CustomerName { get; set; } = null!;

        public string PCateName { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public string ProductDesc { get; set; } = null!;

        public decimal? ProductPrice { get; set; }

        public decimal? Discount { get; set; }

        public string Location { get; set; } = null!;

        public string PurchaseType { get; set; } = null!;

        public DateOnly? PurchaseDate { get; set; }

        public decimal? PercentageOfDamage { get; set; }

        public string? DamageDetail { get; set; }
        public string Status { get; set; } = null!;
        public string Images { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public DateOnly CreatedAt { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
