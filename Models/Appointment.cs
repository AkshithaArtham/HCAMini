<<<<<<< HEAD
﻿namespace HCAMiniEHR.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public int? DoctorId { get; set; }  // Nullable in case no doctor is assigned
        public Doctor Doctor { get; set; }
        public ICollection<LabOrder> LabOrders { get; set; }
    }
}
=======
﻿namespace HCAMiniEHR.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public int? DoctorId { get; set; }  // Nullable in case no doctor is assigned
        public Doctor Doctor { get; set; }
        public ICollection<LabOrder> LabOrders { get; set; }
    }
}
>>>>>>> de0c1979792b9fba70a0d3608ff20cf61cb6a43b
