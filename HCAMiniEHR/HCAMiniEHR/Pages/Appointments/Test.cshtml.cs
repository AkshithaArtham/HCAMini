using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HCAMiniEHR.Pages.Appointments
{
    public class TestModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TestModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<string> TestResults { get; set; } = new List<string>();
        public List<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

        public async Task OnGetAsync()
        {
            await RunTests();
        }

        private async Task RunTests()
        {
            TestResults.Clear();

            // Test 1: Check if patients exist
            var patients = await _context.Patients.ToListAsync();
            TestResults.Add($"Test 1 - Patients in DB: {patients.Count}");

            // Test 2: Check if doctors exist
            var doctors = await _context.Doctors.ToListAsync();
            TestResults.Add($"Test 2 - Doctors in DB: {doctors.Count}");

            // Test 3: Check appointments count
            var appointments = await _context.Appointments.ToListAsync();
            TestResults.Add($"Test 3 - Appointments in DB: {appointments.Count}");

            // Test 4: Check audit logs count
            // Test 4: Check audit logs count
            try
            {
                // Check if AuditLogs table exists
                var tableExists = await _context.Database
                    .SqlQueryRaw<int>("SELECT CASE WHEN OBJECT_ID('Healthcare.AuditLogs', 'U') IS NOT NULL THEN 1 ELSE 0 END")
                    .FirstOrDefaultAsync();

                if (tableExists == 1)
                {
                    AuditLogs = await _context.AuditLogs
                        .Where(a => a.TableName == "Appointments")
                        .OrderByDescending(a => a.ChangedAt)
                        .Take(10)
                        .ToListAsync();
                    TestResults.Add($"Test 4 - Appointment Audit Logs: {AuditLogs.Count}");
                }
                else
                {
                    TestResults.Add("Test 4 - AuditLogs table does not exist. Run the SQL script to create it.");
                }
            }
            catch (Exception ex)
            {
                TestResults.Add($"Test 4 - Error checking audit logs: {ex.Message}");
            }

            // Test 5: Test stored procedure with valid data
            if (patients.Any() && doctors.Any())
            {
                var patientId = patients.First().PatientId;
                var doctorId = doctors.First().DoctorId;
                var futureDate = DateTime.Now.AddDays(1);

                try
                {
                    var parameters = new[]
                    {
                        new SqlParameter("@PatientId", patientId),
                        new SqlParameter("@DoctorId", doctorId),
                        new SqlParameter("@AppointmentDateTime", futureDate),
                        new SqlParameter("@Reason", "Test Appointment"),
                        new SqlParameter("@Notes", "Test from audit page")
                    };

                    var result = await _context.Database
                        .SqlQueryRaw<int>("EXEC [Healthcare].[CreateAppointment] @PatientId, @DoctorId, @AppointmentDateTime, @Reason, @Notes",
                            parameters)
                        .ToListAsync();

                    var appointmentId = result.FirstOrDefault();

                    if (appointmentId > 0)
                    {
                        TestResults.Add($"Test 5 - Stored Procedure Success: Created Appointment ID {appointmentId}");

                        // Clean up test appointment
                        var testAppointment = await _context.Appointments.FindAsync(appointmentId);
                        if (testAppointment != null)
                        {
                            _context.Appointments.Remove(testAppointment);
                            await _context.SaveChangesAsync();
                            TestResults.Add($"Test 5 Cleanup: Removed test appointment {appointmentId}");
                        }
                    }
                }
                catch (SqlException ex)
                {
                    TestResults.Add($"Test 5 - Stored Procedure Error: {ex.Message}");
                }
            }
            else
            {
                TestResults.Add("Test 5 Skipped: Need at least 1 patient and 1 doctor");
            }
        }

        public async Task<IActionResult> OnPostTriggerAuditAsync()
        {
            // Check if AuditLogs table exists
            try
            {
                var tableExists = await _context.Database
                    .SqlQueryRaw<int>("SELECT CASE WHEN OBJECT_ID('Healthcare.AuditLogs', 'U') IS NOT NULL THEN 1 ELSE 0 END")
                    .FirstOrDefaultAsync();

                if (tableExists != 1)
                {
                    TempData["Message"] = "AuditLogs table does not exist. Please run the SQL script to create it first.";
                    return RedirectToPage();
                }

                // Create and immediately update an appointment to trigger audit
                if (_context.Patients.Any() && _context.Doctors.Any())
                {
                    var patient = await _context.Patients.FirstAsync();
                    var doctor = await _context.Doctors.FirstAsync();

                    // Create appointment
                    var appointment = new Appointment
                    {
                        PatientId = patient.PatientId,
                        DoctorId = doctor.DoctorId,
                        AppointmentDateTime = DateTime.Now.AddDays(2),
                        Reason = "Audit Test Appointment",
                        Status = "Scheduled"
                    };

                    _context.Appointments.Add(appointment);
                    await _context.SaveChangesAsync();

                    // Update appointment to trigger UPDATE audit
                    appointment.Reason = "Updated Audit Test Appointment";
                    await _context.SaveChangesAsync();

                    // Delete appointment to trigger DELETE audit
                    _context.Appointments.Remove(appointment);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "Audit test completed - Created, Updated, and Deleted an appointment.";
                }
                else
                {
                    TempData["Message"] = "Need at least 1 patient and 1 doctor to run audit test.";
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error during audit test: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}