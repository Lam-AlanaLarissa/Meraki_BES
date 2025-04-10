using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class CreateFeedbackDTO
    {
        [Required(ErrorMessage = "Product is required")]
        public string FlowerID { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public double Rating { get; set; } = 5;
        [JsonIgnore]
        public string Detail { get; set; }
        [JsonIgnore]
        public bool IsGoodReview { get; set; }
    }
}
