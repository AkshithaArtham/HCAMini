using System;
using System.ComponentModel.DataAnnotations;

namespace HCAMiniEHR.Models.Validations
{
    public class FutureDateAttribute : ValidationAttribute
    {
        private readonly int _maxMonthsAhead;

        public FutureDateAttribute(int maxMonthsAhead)
        {
            _maxMonthsAhead = maxMonthsAhead;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateTime)
            {
                var maxAllowedDate = DateTime.UtcNow.AddMonths(_maxMonthsAhead);

                if (dateTime > maxAllowedDate)
                {
                    return new ValidationResult($"Appointment cannot be more than {_maxMonthsAhead} month(s) in the future.");
                }
            }

            return ValidationResult.Success;
        }
    }
}