using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HCAMiniEHR.Pages.Patients
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // SIMPLE properties - NO Patient object binding
        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Gender { get; set; }

        [BindProperty]
        public DateTime DateOfBirth { get; set; } = DateTime.Today;

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            // SIMPLE validation
            if (string.IsNullOrEmpty(Name))
            {
                ModelState.AddModelError("Name", "Name is required");
                return Page();
            }

            if (string.IsNullOrEmpty(Gender))
            {
                ModelState.AddModelError("Gender", "Gender is required");
                return Page();
            }

            try
            {
                // Create patient object manually
                var patient = new Patient
                {
                    Name = Name,
                    Gender = Gender,
                    DateOfBirth = DateOfBirth
                };

                _context.Patients.Add(patient);
                _context.SaveChanges();

                Console.WriteLine($"Patient created: {Name}, {Gender}, {DateOfBirth}");

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return Page();
            }
        }
    }
}