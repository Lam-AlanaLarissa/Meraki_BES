using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models;

public partial class Customer
{
    [Key] public string CustomerId { get; set; } = null!;

    [ForeignKey("AccountId")] public string AccountId { get; set; } = null!;

    public string? Avatar { get; set; }

    public string? CardName { get; set; }

    public long? CardNumber { get; set; }

    [ForeignKey("CardProviderName")] public string? CardProviderName { get; set; }

    public long? TaxNumber { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual CardProvider? CardProviderNameNavigation { get; set; }
}
