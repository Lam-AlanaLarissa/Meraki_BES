using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Models;

public partial class CardProvider
{
    [Key]public string CardProviderName { get; set; } = null!;

    public string CpfullName { get; set; } = null!;

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
