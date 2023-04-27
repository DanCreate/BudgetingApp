using System.ComponentModel.DataAnnotations;

namespace BudgetingApp.Validation
{
    public class FirstLetterUpperAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            var firstletter = value.ToString()[0].ToString();

            if (firstletter != firstletter.ToUpper())
            {
                return new ValidationResult("First letter must be upper case");
            }

            return ValidationResult.Success;   
        }

    }
}
