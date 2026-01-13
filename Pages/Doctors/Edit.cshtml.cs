<<<<<<< HEAD
using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HCAMiniEHR.Pages.Doctors
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Doctor Doctor { get; set; }

        // GET: /Doctors/Edit/10
        public IActionResult OnGet(int id)
        {
            // Fetch the doctor based on the id from the route
            Doctor = _context.Doctors.FirstOrDefault(d => d.DoctorId == id);

            if (Doctor == null)
            {
                // Return NotFound if doctor is not found
                return NotFound();
            }

            return Page();
        }

        // POST: /Doctors/Edit/10
        public IActionResult OnPost(int id)  // Added id parameter
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Find the doctor by the id parameter
                var doctorInDb = _context.Doctors.FirstOrDefault(d => d.DoctorId == id);

                if (doctorInDb == null)
                {
                    // Handle case where doctor is not found in the database
                    return NotFound();
                }

                // Update the properties from the bound Doctor object
                doctorInDb.Name = Doctor.Name;
                doctorInDb.Specialty = Doctor.Specialty;
                doctorInDb.Gender = Doctor.Gender;

                // Save changes to the database
                _context.SaveChanges();

                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency issues
                if (!DoctorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes. The doctor was deleted or modified by another user.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return Page();
            }
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.DoctorId == id);
        }
    }
=======
using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HCAMiniEHR.Pages.Doctors
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Doctor Doctor { get; set; }

        // GET: /Doctors/Edit/10
        public IActionResult OnGet(int id)
        {
            // Fetch the doctor based on the id from the route
            Doctor = _context.Doctors.FirstOrDefault(d => d.DoctorId == id);

            if (Doctor == null)
            {
                // Return NotFound if doctor is not found
                return NotFound();
            }

            return Page();
        }

        // POST: /Doctors/Edit/10
        public IActionResult OnPost(int id)  // Added id parameter
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Find the doctor by the id parameter
                var doctorInDb = _context.Doctors.FirstOrDefault(d => d.DoctorId == id);

                if (doctorInDb == null)
                {
                    // Handle case where doctor is not found in the database
                    return NotFound();
                }

                // Update the properties from the bound Doctor object
                doctorInDb.Name = Doctor.Name;
                doctorInDb.Specialty = Doctor.Specialty;
                doctorInDb.Gender = Doctor.Gender;

                // Save changes to the database
                _context.SaveChanges();

                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency issues
                if (!DoctorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes. The doctor was deleted or modified by another user.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return Page();
            }
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.DoctorId == id);
        }
    }
>>>>>>> de0c1979792b9fba70a0d3608ff20cf61cb6a43b
}