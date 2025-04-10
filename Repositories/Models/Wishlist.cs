using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Models;

public partial class Wishlist
{
    [Key] public string WishId { get; set; } = null!;

    public string ProductId { get; set; } = null!;
}
