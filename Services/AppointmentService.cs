using System.Collections.Generic;
using System.Linq;
using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;


namespace HCAMiniEHR.Services
{
    public class AppointmentService
    {
        private readonly ApplicationDbContext _context;

        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Appointment> GetAll()
        {
            return _context.Appointments
                           .Include(a => a.Patient)
                           .ToList();
        }

        public void Add(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            _context.SaveChanges();
        }
        public void AddUsingStoredProcedure(Appointment appointment)
        {
            var patientIdParam = new SqlParameter("@PatientId", appointment.PatientId);
            var dateParam = new SqlParameter("@AppointmentDate", appointment.AppointmentDate);
            var statusParam = new SqlParameter("@Status", appointment.Status);

            _context.Database.ExecuteSqlRaw(
                "EXEC Healthcare.CreateAppointment @PatientId, @AppointmentDate, @Status",
                patientIdParam,
                dateParam,
                statusParam
            );
        }
        public Appointment? GetById(int id)
        {
            return _context.Appointments
                .Include(a => a.Patient)
                .FirstOrDefault(a => a.AppointmentId == id);
        }

        public void Update(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                _context.SaveChanges();
            }
        }


    }
}
