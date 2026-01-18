using System;

namespace HCAMiniEHR.Models.DTOs
{
    // DTO for Pending Lab Orders Report
    public class PendingLabOrderReportDTO
    {
        public int LabOrderId { get; set; }
        public string PatientName { get; set; }
        public string TestName { get; set; }
        public DateTime OrderDate { get; set; }
        public int DaysPending { get; set; }
        public string Notes { get; set; }
    }

    // DTO for Patients Without Follow-Up Report
    public class PatientWithoutFollowupReportDTO
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public DateTime? LastAppointmentDate { get; set; }
        public string LastAppointmentReason { get; set; }
        public int DaysSinceLastAppointment { get; set; }
    }

    // DTO for Doctor Productivity Report
    public class DoctorProductivityReportDTO
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Specialty { get; set; }
        public int TotalAppointments { get; set; }
        public int CompletedAppointments { get; set; }
        public int CancelledAppointments { get; set; }
        public double CompletionRate { get; set; }
        public DateTime? LastAppointmentDate { get; set; }
    }
}