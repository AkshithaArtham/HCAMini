namespace HCAMiniEHR.Models
{
    public class LabOrder
    {
        public int LabOrderId { get; set; }
        public string TestName { get; set; }
        public string Status { get; set; }
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
    }
}
