using Humanizer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$",
        ErrorMessage = "Password must have at least 6 characters, an uppercase letter, and a special character")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Fullname is required")]
        public string Fullname { get; set; } = null!;

        public string Username { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^((\(84\)|84)?0?|0)?\d{9}$", ErrorMessage = "Invalid phone number format")]
        public long Phone { get; set; }

        [Required(ErrorMessage = "Birthday is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]

        public DateTime Birthday { get; set; }
        public string Status { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

    }
}
