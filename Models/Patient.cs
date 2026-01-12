using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCAMiniEHR.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        [Required]
        public string Gender { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
    }
}
