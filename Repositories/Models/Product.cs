using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models;

public partial class Product
{
    [Key] public string ProductId { get; set; } = null!;

    [ForeignKey("AccountID")] public string AccountId { get; set; } = null!;

    public string PcateId { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string ProductDesc { get; set; } = null!;

    public decimal? ProductPrice { get; set; }

    public decimal? Discount { get; set; }
    public double StockQuantity { get; set; } = 1;

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

    public virtual Account Account { get; set; } = null!;

    public virtual ProductCategory Pcate { get; set; } = null!;
    public virtual ICollection<Feedback> Feedbacks { get; set; }
    public virtual ICollection<Report> Reports { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }


}
