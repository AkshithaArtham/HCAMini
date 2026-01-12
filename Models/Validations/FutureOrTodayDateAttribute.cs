using System;
using System.ComponentModel.DataAnnotations;

namespace HCAMiniEHR.Models.Validations
{
    public class FutureOrTodayDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var date = (DateTime)value;

            if (date.Date < DateTime.Today)
            {
                return new ValidationResult("Appointment date cannot be in the past.");
            }

            return ValidationResult.Success;
        }
    }
}
