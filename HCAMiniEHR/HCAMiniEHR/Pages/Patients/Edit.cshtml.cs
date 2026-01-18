using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HCAMiniEHR.Pages.Patients
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // SIMPLE properties - same as Create
        [BindProperty]
        public int PatientId { get; set; }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Gender { get; set; }

        [BindProperty]
        public DateTime DateOfBirth { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            // Load data into properties
            PatientId = patient.PatientId;
            Name = patient.Name;
            Gender = patient.Gender;
            DateOfBirth = patient.DateOfBirth;

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var patient = new Patient
            {
                PatientId = PatientId,
                Name = Name,
                Gender = Gender,
                DateOfBirth = DateOfBirth
            };

            _context.Attach(patient).State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}