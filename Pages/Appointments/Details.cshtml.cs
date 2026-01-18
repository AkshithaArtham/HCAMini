using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCAMiniEHR.Pages.Appointments
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Appointment Appointment { get; set; }
        public List<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || id == 0)
            {
                // Try to get ID from query string if not in route
                if (Request.Query.ContainsKey("id") && int.TryParse(Request.Query["id"], out int queryId))
                {
                    id = queryId;
                }
                else
                {
                    return NotFound();
                }
            }

            // Load appointment with related data
            Appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);

            if (Appointment == null)
            {
                return NotFound();
            }

            // Safely load audit logs (handle null case)
            try
            {
                AuditLogs = await _context.AuditLogs
                    .Where(a => a.TableName == "Appointments" && a.RecordId == id.Value)
                    .OrderByDescending(a => a.ChangedAt)
                    .ToListAsync();
            }
            catch
            {
                // If audit logs fail, just continue without them
                AuditLogs = new List<AuditLog>();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCompleteAsync(int id)
        {
            if (id <= 0) return NotFound();

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.Status = "Completed";
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Details", new { id = id });
        }

        public async Task<IActionResult> OnPostCancelAsync(int id)
        {
            if (id <= 0) return NotFound();

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.Status = "Cancelled";
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Details", new { id = id });
        }
    }
}