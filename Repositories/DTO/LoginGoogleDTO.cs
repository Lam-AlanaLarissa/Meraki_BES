using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class LoginGoogleDTO
    {
        [Required(ErrorMessage = "The token is required")]
        public string FirebaseToken { get; set; }
    }
}
