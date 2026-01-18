using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Models.DTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using HCAMiniEHR.Models.DTOs;  // Add this line

namespace HCAMiniEHR.Pages.Reports
{
    public class PatientsWithoutFollowupModel : PageModel
    {
        private readonly string _connectionString;

        public List<PatientWithoutFollowupReportDTO> Patients { get; set; } = new List<PatientWithoutFollowupReportDTO>();
        public int TotalPatients { get; set; }

        public PatientsWithoutFollowupModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task OnGetAsync()
        {
            await LoadPatientsWithoutFollowup();
            TotalPatients = Patients.Count;
        }

        private async Task LoadPatientsWithoutFollowup()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = @"
            -- Get patients who have NO future appointments
            SELECT 
                p.PatientId,
                p.Name as PatientName,
                -- Get last appointment date
                (SELECT MAX(AppointmentDateTime) 
                 FROM [Healthcare].[Appointments] a 
                 WHERE a.PatientId = p.PatientId) as LastAppointmentDate,
                -- Get reason for last appointment
                (SELECT TOP 1 Reason 
                 FROM [Healthcare].[Appointments] a 
                 WHERE a.PatientId = p.PatientId 
                 ORDER BY AppointmentDateTime DESC) as LastAppointmentReason
            FROM [Healthcare].[Patients] p
            WHERE NOT EXISTS (
                SELECT 1 
                FROM [Healthcare].[Appointments] a 
                WHERE a.PatientId = p.PatientId 
                  AND a.AppointmentDateTime > GETDATE() 
                  AND a.Status != 'Cancelled'
            )
            ORDER BY LastAppointmentDate ASC";

                using (var command = new SqlCommand(sql, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var patient = new PatientWithoutFollowupReportDTO
                        {
                            PatientId = reader.GetInt32("PatientId"),
                            PatientName = reader["PatientName"] as string ?? "Unknown"
                        };

                        // Handle nullable LastAppointmentDate
                        if (!reader.IsDBNull("LastAppointmentDate"))
                        {
                            patient.LastAppointmentDate = reader.GetDateTime("LastAppointmentDate");
                            patient.DaysSinceLastAppointment = (int)(DateTime.Today - patient.LastAppointmentDate.Value.Date).TotalDays;
                            patient.LastAppointmentReason = reader["LastAppointmentReason"] as string;
                        }
                        else
                        {
                            patient.DaysSinceLastAppointment = 999; // Never had appointment
                        }

                        Patients.Add(patient);
                    }
                }
            }
        }
    }
}