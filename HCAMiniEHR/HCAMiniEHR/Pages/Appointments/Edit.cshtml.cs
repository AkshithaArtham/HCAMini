using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HCAMiniEHR.Pages.Appointments
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Appointment Appointment { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please select a patient")]
        public int? SelectedPatientId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please select a doctor")]
        public int? SelectedDoctorId { get; set; }

        public SelectList Patients { get; set; }
        public SelectList Doctors { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Load appointment
            Appointment = await _context.Appointments
                .FirstOrDefaultAsync(m => m.AppointmentId == id);

            if (Appointment == null)
            {
                return NotFound();
            }

            // Set the selected IDs
            SelectedPatientId = Appointment.PatientId;
            SelectedDoctorId = Appointment.DoctorId;

            await LoadDropdowns();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Console.WriteLine($"=== DEBUG: EDIT APPOINTMENT {id} ===");

            // Load existing appointment
            var existingAppointment = await _context.Appointments.FindAsync(id);
            if (existingAppointment == null)
            {
                return NotFound();
            }

            // Validate required fields
            var errors = new List<string>();

            if (!SelectedPatientId.HasValue || SelectedPatientId.Value == 0)
            {
                errors.Add("Please select a patient");
                ModelState.AddModelError("SelectedPatientId", "Please select a patient");
            }

            if (!SelectedDoctorId.HasValue || SelectedDoctorId.Value == 0)
            {
                errors.Add("Please select a doctor");
                ModelState.AddModelError("SelectedDoctorId", "Please select a doctor");
            }

            if (string.IsNullOrWhiteSpace(Request.Form["Appointment.AppointmentDateTime"]))
            {
                errors.Add("Appointment date and time is required");
                ModelState.AddModelError("Appointment.AppointmentDateTime", "Appointment date and time is required");
            }

            if (string.IsNullOrWhiteSpace(Request.Form["Appointment.Reason"]))
            {
                errors.Add("Reason is required");
                ModelState.AddModelError("Appointment.Reason", "Reason is required");
            }

            if (errors.Any())
            {
                Console.WriteLine("=== DEBUG: VALIDATION ERRORS ===");
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }

                // Re-load appointment data
                Appointment = existingAppointment;
                await LoadDropdowns();
                return Page();
            }

            // Parse the date from form
            DateTime appointmentDateTime;
            if (!DateTime.TryParse(Request.Form["Appointment.AppointmentDateTime"], out appointmentDateTime))
            {
                ModelState.AddModelError("Appointment.AppointmentDateTime", "Invalid date format");
                Appointment = existingAppointment;
                await LoadDropdowns();
                return Page();
            }

            // Update the existing appointment
            existingAppointment.PatientId = SelectedPatientId.Value;
            existingAppointment.DoctorId = SelectedDoctorId.Value;
            existingAppointment.AppointmentDateTime = appointmentDateTime;
            existingAppointment.Reason = Request.Form["Appointment.Reason"];
            existingAppointment.Notes = Request.Form["Appointment.Notes"];
            existingAppointment.Status = Request.Form["Appointment.Status"];

            try
            {
                await _context.SaveChangesAsync();
                Console.WriteLine($"=== DEBUG: APPOINTMENT {id} UPDATED SUCCESSFULLY ===");

                TempData["SuccessMessage"] = "Appointment updated successfully!";
                return RedirectToPage("./Details", new { id = id });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"=== DEBUG: CONCURRENCY ERROR ===");
                Console.WriteLine($"Error: {ex.Message}");

                if (!AppointmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "The appointment was modified by another user. Please refresh and try again.");
                    await LoadDropdowns();
                    return Page();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"=== DEBUG: GENERAL ERROR ===");
                Console.WriteLine($"Error: {ex.Message}");

                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                await LoadDropdowns();
                return Page();
            }
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentId == id);
        }

        private async Task LoadDropdowns()
        {
            var patients = await _context.Patients
                .OrderBy(p => p.Name)
                .ToListAsync();

            var doctors = await _context.Doctors
                .OrderBy(d => d.Name)
                .ToListAsync();

            Patients = new SelectList(patients, "PatientId", "Name");
            Doctors = new SelectList(doctors, "DoctorId", "Name");
        }
    }
}