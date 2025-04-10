using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class UpdateFeedbackDTO
    {
        public string Detail { get; set; }
        public double Rating { get; set; }
        public IFormFile? Attachments { get; set; }
    }
}
