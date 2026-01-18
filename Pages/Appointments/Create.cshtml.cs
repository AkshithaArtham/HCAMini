using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
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
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Appointment Appointment { get; set; } = new Appointment();

        [BindProperty]
        [Required(ErrorMessage = "Please select a patient")]
        public int? SelectedPatientId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please select a doctor")]
        public int? SelectedDoctorId { get; set; }

        public SelectList Patients { get; set; }
        public SelectList Doctors { get; set; }

        public async Task OnGetAsync()
        {
            await LoadDropdowns();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // DEBUG: Log form values
            Console.WriteLine("=== DEBUG: FORM VALUES RECEIVED ===");
            Console.WriteLine($"SelectedPatientId: {SelectedPatientId?.ToString() ?? "NULL"}");
            Console.WriteLine($"SelectedDoctorId: {SelectedDoctorId?.ToString() ?? "NULL"}");

            // MANUALLY validate required fields
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
                Console.WriteLine("================================");

                await LoadDropdowns();
                return Page();
            }

            // Parse the date from form
            DateTime appointmentDateTime;
            if (!DateTime.TryParse(Request.Form["Appointment.AppointmentDateTime"], out appointmentDateTime))
            {
                ModelState.AddModelError("Appointment.AppointmentDateTime", "Invalid date format");
                await LoadDropdowns();
                return Page();
            }

            // Create Appointment object from form data
            var appointment = new Appointment
            {
                PatientId = SelectedPatientId.Value,
                DoctorId = SelectedDoctorId.Value,
                AppointmentDateTime = appointmentDateTime,
                Reason = Request.Form["Appointment.Reason"],
                Notes = Request.Form["Appointment.Notes"],
                Status = "Scheduled",
                CreatedDate = DateTime.UtcNow
            };

            Console.WriteLine("=== DEBUG: CALLING STORED PROCEDURE ===");
            Console.WriteLine($"PatientId: {appointment.PatientId}");
            Console.WriteLine($"DoctorId: {appointment.DoctorId}");
            Console.WriteLine($"AppointmentDateTime: {appointment.AppointmentDateTime}");
            Console.WriteLine($"Reason: {appointment.Reason}");
            Console.WriteLine("=======================================");

            try
            {
                var parameters = new[]
                {
            new SqlParameter("@PatientId", appointment.PatientId),
            new SqlParameter("@DoctorId", appointment.DoctorId),
            new SqlParameter("@AppointmentDateTime", appointment.AppointmentDateTime),
            new SqlParameter("@Reason", appointment.Reason),
            new SqlParameter("@Notes", appointment.Notes ?? (object)DBNull.Value)
        };

                var result = await _context.Database
                    .SqlQueryRaw<int>("EXEC [Healthcare].[CreateAppointment] @PatientId, @DoctorId, @AppointmentDateTime, @Reason, @Notes",
                        parameters)
                    .ToListAsync();

                var appointmentId = result.FirstOrDefault();

                if (appointmentId > 0)
                {
                    Console.WriteLine($"=== DEBUG: APPOINTMENT CREATED SUCCESSFULLY ===");
                    Console.WriteLine($"New Appointment ID: {appointmentId}");
                    Console.WriteLine("=============================================");

                    TempData["SuccessMessage"] = $"Appointment created successfully with ID: {appointmentId}";
                    return RedirectToPage("./Index");
                }
                else
                {
                    Console.WriteLine("=== DEBUG: STORED PROCEDURE RETURNED 0 ===");
                    ModelState.AddModelError(string.Empty, "Failed to create appointment. No appointment ID returned.");
                    await LoadDropdowns();
                    return Page();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("=== DEBUG: SQL EXCEPTION ===");
                Console.WriteLine($"Error Number: {ex.Number}");
                Console.WriteLine($"Error Message: {ex.Message}");
                Console.WriteLine("============================");

                ModelState.AddModelError(string.Empty, $"Database error: {ex.Message}");
                await LoadDropdowns();
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine("=== DEBUG: GENERAL EXCEPTION ===");
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                Console.WriteLine("================================");

                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                await LoadDropdowns();
                return Page();
            }
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