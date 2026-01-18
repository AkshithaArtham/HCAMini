using HCAMiniEHR.Models;
using System.Linq;

namespace HCAMiniEHR.Data
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            // Seed Doctors
            if (!context.Doctors.Any())
            {
                Console.WriteLine("Seeding Doctors...");
                context.Doctors.AddRange(
                    new Doctor { Name = "Dr. John Doe", Specialty = "Cardiologist", Gender = "Male" },
                    new Doctor { Name = "Dr. Jane Smith", Specialty = "Neurologist", Gender = "Female" }
                );
                context.SaveChanges();
            }

            // Seed Patients
            if (!context.Patients.Any())
            {
                Console.WriteLine("Seeding Patients...");
                context.Patients.AddRange(
                    new Patient { Name = "Alice Johnson", Gender = "Female", DateOfBirth = new DateTime(1990, 5, 12) },
                    new Patient { Name = "Bob Williams", Gender = "Male", DateOfBirth = new DateTime(1985, 8, 24) }
                );
                context.SaveChanges();
            }

            // Seed Appointments
            //if (!context.Appointments.Any())
            //{
            //    Console.WriteLine("Seeding Appointments...");
            //    var doctor = context.Doctors.First();
            //    var patient = context.Patients.First();

            //    context.Appointments.AddRange(
            //        new Appointment { AppointmentDate = DateTime.Now.AddDays(2), Status = "Scheduled", PatientId = patient.PatientId, DoctorId = doctor.DoctorId },
            //        new Appointment { AppointmentDate = DateTime.Now.AddDays(5), Status = "Scheduled", PatientId = patient.PatientId, DoctorId = doctor.DoctorId }
            //    );
            //    context.SaveChanges();
            //}

            //// Seed Lab Orders
            //if (!context.LabOrders.Any())
            //{
            //    Console.WriteLine("Seeding Lab Orders...");
            //    //var appointment = context.Appointments.First();
            //    context.LabOrders.Add(new LabOrder { TestName = "Blood Test", Status = "Pending", AppointmentId = appointment.AppointmentId });
            //    context.SaveChanges();
            //}
        }

    }
}
