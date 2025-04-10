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
        [Required(ErrorMessage = "AccessToken is required")]
        public string accessToken { get; set; }
        [Required(ErrorMessage = "FlowerId is required")]
        public string FlowerID { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [DefaultValue(1)]
        public double Quantity { get; set; }
    }
}
