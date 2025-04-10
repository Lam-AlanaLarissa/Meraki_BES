using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class UpdateAccountProfileDTO
    {
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;

        //[Required(ErrorMessage = "Password is required")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$",
        //ErrorMessage = "Password must have at least 6 characters, an uppercase letter, and a special character")]
        //public string Password { get; set; } = null!;

        public string Fullname { get; set; } = null!;

        public string Username { get; set; }

        [RegularExpression(@"^((\(84\)|84)?0?|0)?\d{9}$", ErrorMessage = "Invalid phone number format")]
        public long Phone { get; set; }



        public string Address { get; set; } = null!;

        public string Gender { get; set; }
        public IFormFile AttachmentFile { get; set; }
        //public string CardName { get; set; }

        //public long? CardNumber { get; set; }

        //public string CardProviderName { get; set; }

        //public long? TaxNumber { get; set; }
    }
}
