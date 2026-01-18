using System;

namespace HCAMiniEHR.Models.DTOs
{
    // DTO for creating Lab Order (input for stored procedure)
    public class CreateLabOrderDTO
    {
        public int PatientId { get; set; }
        public int? AppointmentId { get; set; }
        public string TestName { get; set; }
        public string Notes { get; set; }
    }

    // DTO for displaying Lab Order in lists
    public class LabOrderDisplayDTO
    {
        public int LabOrderId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int? AppointmentId { get; set; }
        public string TestName { get; set; }
        public string Status { get; set; }
        public DateTime? OrderDate { get; set; }
        public string Notes { get; set; }
    }

    // DTO for Lab Order Details
    public class LabOrderDetailDTO
    {
        public int LabOrderId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int? AppointmentId { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string TestName { get; set; }
        public string Status { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ResultDate { get; set; }
        public string Notes { get; set; }
        public string ResultNotes { get; set; }
        public string DoctorName { get; set; }
    }

    // DTO for Audit Log
    public class AuditLogDTO
    {
        public int AuditLogId { get; set; }
        public string Action { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public DateTime ChangedAt { get; set; }
        public string ChangedBy { get; set; }
    }
}