using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models;

public partial class CustomerWallet
{
    [Key]public string WalletId { get; set; } = null!;

    [ForeignKey("AccountId")] public string AccountId { get; set; } = null!;

    public decimal Balance { get; set; }

    public virtual Account Account { get; set; } = null!;
}
