// Models/LabOrder.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCAMiniEHR.Models
{
    [Table("LabOrder", Schema = "Healthcare")]
    public class LabOrder
    {
        [Key]
        public int LabOrderId { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        [ForeignKey("AppointmentId")]
        public virtual Appointment Appointment { get; set; }

        // Add these if you want direct Patient/Doctor references in LabOrder
        public int PatientId { get; set; } // Add this
        public int DoctorId { get; set; }  // Add this

        [Required]
        [StringLength(50)]
        public string TestType { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Cost { get; set; }

        public DateTime OrderedDate { get; set; } = DateTime.Now;
        public DateTime? CompletedDate { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        // Navigation properties for direct access
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }
    }
}