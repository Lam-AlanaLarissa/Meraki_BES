using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class RegisterByGoogleDTO
    {
        [Required(ErrorMessage = "Firebase Token is required")]
        public string FirebaseToken { get; set; }
        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^((\(84\)|84)?0?|0)?\d{9}$", ErrorMessage = "Invalid phone number format")]
        public long Phone { get; set; }//sua lai

        [Required(ErrorMessage = "Birthday is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        public DateTime Birthday { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Birthday > DateTime.Now)
        //    {
        //        yield return new ValidationResult("Birthday cannot be in the future.", new[] { nameof(Birthday) });
        //    }
        //}
        //SUA LAI

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }



    }
}
