using System.ComponentModel.DataAnnotations;

namespace HCAMiniEHR.Models
{
    public class LabOrder
    {
        public int LabOrderId { get; set; }

        [Required]
        public string TestName { get; set; }

        [Required]
        public string Status { get; set; }

        // FK
        [Required]
        public int AppointmentId { get; set; }

        // Navigation
        public Appointment? Appointment { get; set; }
    }
}
