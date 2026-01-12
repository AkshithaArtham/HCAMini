using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HCAMiniEHR.Models.Validations;

namespace HCAMiniEHR.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Appointment date is required.")]
        [FutureOrTodayDate(ErrorMessage = "Appointment date cannot be in the past.")]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public int PatientId { get; set; }

        public Patient? Patient { get; set; }

        public ICollection<LabOrder>? LabOrders { get; set; }
    }
}
