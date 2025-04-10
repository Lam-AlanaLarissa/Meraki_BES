using Microsoft.Extensions.DependencyInjection;
using Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Services.Common
{
    public class UniqueShopNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var shopName = value as string;
            if (string.IsNullOrEmpty(shopName))
            {
                return new ValidationResult("Shop name is required.");
            }

            // Access the service provider to resolve the dependency
            var _accountRepository = validationContext.GetService<IAccountRepository>();
            //var existingShop = _accountService.GetUserByShopName(shopName).Result;

            //if (existingShop != null)
            //{
            //    return new ValidationResult("This shop name already exists. Please try another.");
            //}

            return ValidationResult.Success;
        }
    }
}
