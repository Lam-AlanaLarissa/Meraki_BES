using System.ComponentModel.DataAnnotations;

namespace Services.Common
{
    public class DateAfterAttribute : ValidationAttribute
    {
        private readonly string _startDatePropertyName;

        public DateAfterAttribute(string startDatePropertyName)
        {
            _startDatePropertyName = startDatePropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var endDate = (DateTime?)value;
            var startDateProperty = validationContext.ObjectType.GetProperty(_startDatePropertyName);

            if (startDateProperty == null)
                return new ValidationResult($"Unknown property: {_startDatePropertyName}");

            var startDate = (DateTime?)startDateProperty.GetValue(validationContext.ObjectInstance);

            if (startDate != null && endDate != null && endDate <= startDate)
                return new ValidationResult(ErrorMessage ?? "End Time must be after Start Time.");

            return ValidationResult.Success;
        }
    }
}
