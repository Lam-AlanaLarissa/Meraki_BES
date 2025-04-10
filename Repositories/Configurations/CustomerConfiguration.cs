using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Configurations
{

    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasData(
                new Customer { CustomerId = "C000000001", AccountId = "AC00000001", Avatar = null, CardName = "Visa Platinum", CardNumber = 1234567890123456, CardProviderName = "Vietcombank", TaxNumber = 123456789 },
                new Customer { CustomerId = "C000000002", AccountId = "AC00000003", Avatar = null, CardName = "MasterCard Gold", CardNumber = 9876543210987654, CardProviderName = "BIDV", TaxNumber = 987654321 },
                new Customer { CustomerId = "C000000003", AccountId = "AC00000005", Avatar = null, CardName = "JCB Standard", CardNumber = 4561237894561237, CardProviderName = "Techcombank", TaxNumber = 456123789 },
                new Customer { CustomerId = "C000000004", AccountId = "AC00000007", Avatar = null, CardName = "Visa Infinite", CardNumber = 7418529638527418, CardProviderName = "Sacombank", TaxNumber = 741852963 },
                new Customer { CustomerId = "C000000005", AccountId = "AC00000009", Avatar = null, CardName = "MasterCard Standard", CardNumber = 3692581473692581, CardProviderName = "MBBank", TaxNumber = 369258147 }
            );
        }
    }
}
