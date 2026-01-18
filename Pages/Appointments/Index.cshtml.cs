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
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string StatusFilter { get; set; }

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Appointment> Appointments { get; set; }

        public async Task OnGetAsync()
        {
            //Appointments = await _context.Appointments
            //    .Include(a => a.Patient)
            //    .Include(a => a.Doctor)
            //    .OrderByDescending(a => a.AppointmentDateTime)
            //    .ToListAsync();
            IQueryable<Appointment> appointmentsQuery = _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .OrderByDescending(a => a.AppointmentDateTime);

            // Apply search filter
            if (!string.IsNullOrEmpty(SearchString))
            {
                appointmentsQuery = appointmentsQuery.Where(a =>
                    a.Patient.Name.Contains(SearchString) ||
                    a.Doctor.Name.Contains(SearchString) ||
                    a.Reason.Contains(SearchString));
            }

            // Apply status filter
            if (!string.IsNullOrEmpty(StatusFilter))
            {
                appointmentsQuery = appointmentsQuery.Where(a => a.Status == StatusFilter);
            }

            Appointments = await appointmentsQuery.ToListAsync();
        
        }
       
        public async Task<IActionResult> OnPostCompleteAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.Status = "Completed";
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCancelAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.Status = "Cancelled";
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}