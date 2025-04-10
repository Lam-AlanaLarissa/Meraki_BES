using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class AccountProfileResponse
    {
        public string Email { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public long Phone { get; set; }

        public DateTime Birthday { get; set; }

        public string Address { get; set; } = null!;

        public string Gender { get; set; }
        public string Avatar { get; set; }
        public string CardName { get; set; }

        public long? CardNumber { get; set; }

        public string CardProviderName { get; set; }

        public long? TaxNumber { get; set; }


    }
}
