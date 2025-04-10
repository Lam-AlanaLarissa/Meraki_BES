using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Models;

public partial class ProductCategory
{
    [Key] public string PcateId { get; set; } = null!;

    public string PcateName { get; set; } = null!;

    public string PcateDesc { get; set; } = null!;

    public string PcateStatus { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
