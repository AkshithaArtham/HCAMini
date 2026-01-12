using HCAMiniEHR.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HCAMiniEHR.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // REPORT 1: Pending Lab Orders
        public IActionResult PendingLabOrders()
        {
            var report = _context.LabOrders
                .Include(l => l.Appointment)
                .ThenInclude(a => a.Patient)
                .Where(l => l.Status == "Pending")
                .Select(l => new
                {
                    PatientName = l.Appointment.Patient.FullName,
                    AppointmentId = l.AppointmentId,
                    TestName = l.TestName,
                    Status = l.Status
                })
                .ToList();

            return View(report);
        }

        // REPORT 2: Patients without future appointments
        public IActionResult PatientsWithoutFollowUp()
        {
            var report = _context.Patients
                .Where(p => !p.Appointments.Any(a => a.AppointmentDate > DateTime.Now))
                .Select(p => new
                {
                    p.PatientId,
                    p.FullName,
                    p.Gender
                })
                .ToList();

            return View(report);
        }

        // REPORT 3: Appointments grouped by status
        public IActionResult AppointmentStatusSummary()
        {
            var report = _context.Appointments
                .GroupBy(a => a.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToList();

            return View(report);
        }
    }
}
