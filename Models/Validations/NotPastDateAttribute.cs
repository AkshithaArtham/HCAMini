using System;
using System.ComponentModel.DataAnnotations;

namespace HCAMiniEHR.Models.Validations
{
    public class NotPastDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateTime)
            {
                // Allow 30 minutes buffer for timezone/server differences
                if (dateTime < DateTime.UtcNow.AddMinutes(-30))
                {
                    return new ValidationResult("Appointment date cannot be in the past.");
                }
            }

            return ValidationResult.Success;
        }
    }
}