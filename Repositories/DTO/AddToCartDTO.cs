using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class AddToCartDTO
    {

        [Required(ErrorMessage = "ProductId is required")]
        public string productId { get; set; }

        [Required(ErrorMessage = "quantity is required")]
        [DefaultValue(1)]
        public double? quantity { get; set; }
    }
}
