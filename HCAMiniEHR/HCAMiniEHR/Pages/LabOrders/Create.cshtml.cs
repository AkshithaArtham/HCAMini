using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using HCAMiniEHR.Models.DTOs;
using HCAMiniEHR.Services;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HCAMiniEHR.Pages.LabOrders
{
    public class CreateModel : PageModel
    {
        private readonly LabOrderService _labOrderService;
        private readonly string _connectionString;

        [BindProperty]
        public CreateLabOrderDTO LabOrderDTO { get; set; } = new CreateLabOrderDTO();

        public List<SelectListItem> Patients { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Appointments { get; set; } = new List<SelectListItem>();

        public CreateModel(LabOrderService labOrderService, IConfiguration configuration)
        {
            _labOrderService = labOrderService;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task OnGetAsync()
        {
            await LoadDropdowns();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return Page();
            }

            try
            {
                // Use the service to call stored procedure with DTO
                var labOrderId = await _labOrderService.CreateLabOrderAsync(LabOrderDTO);

                TempData["SuccessMessage"] = $"Lab order created successfully with ID: {labOrderId}";
                return RedirectToPage("./Index");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError(string.Empty, $"Database error: {ex.Message}");
                await LoadDropdowns();
                return Page();
            }
        }

        private async Task LoadDropdowns()
        {
            // Load patients
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Load patients
                var patientSql = "SELECT PatientId, Name FROM [Healthcare].[Patients] ORDER BY Name";
                using (var command = new SqlCommand(patientSql, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    Patients.Add(new SelectListItem("-- Select Patient --", ""));
                    while (await reader.ReadAsync())
                    {
                        Patients.Add(new SelectListItem(
                            reader["Name"].ToString(),
                            reader["PatientId"].ToString()
                        ));
                    }
                }

                // Load appointments
                var appointmentSql = @"
                    SELECT a.AppointmentId, p.Name as PatientName, a.AppointmentDateTime 
                    FROM [Healthcare].[Appointments] a
                    JOIN [Healthcare].[Patients] p ON a.PatientId = p.PatientId
                    WHERE a.Status IN ('Scheduled', 'Completed')
                    ORDER BY a.AppointmentDateTime DESC";

                using (var command = new SqlCommand(appointmentSql, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    Appointments.Add(new SelectListItem("-- Select Appointment (Optional) --", ""));
                    while (await reader.ReadAsync())
                    {
                        Appointments.Add(new SelectListItem(
                            $"Appt {reader["AppointmentId"]}: {reader["PatientName"]} - {reader.GetDateTime("AppointmentDateTime"):yyyy-MM-dd HH:mm}",
                            reader["AppointmentId"].ToString()
                        ));
                    }
                }
            }
        }
    }
}