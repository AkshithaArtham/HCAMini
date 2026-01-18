using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Models.DTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HCAMiniEHR.Pages.Reports
{
    public class DoctorProductivityModel : PageModel
    {
        private readonly string _connectionString;

        public List<DoctorProductivityReportDTO> Doctors { get; set; } = new List<DoctorProductivityReportDTO>();
        public DateTime ReportStartDate { get; set; } = DateTime.Today.AddDays(-30);
        public DateTime ReportEndDate { get; set; } = DateTime.Today;

        public DoctorProductivityModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task OnGetAsync(int? days = 30)
        {
            ReportStartDate = DateTime.Today.AddDays(-days.Value);
            await LoadDoctorProductivity(days.Value);
        }

        private async Task LoadDoctorProductivity(int days)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = @"
                    SELECT 
                        d.DoctorId,
                        d.Name as DoctorName,
                        d.Specialty,
                        -- Total appointments in period
                        (SELECT COUNT(*) 
                         FROM [Healthcare].[Appointments] a 
                         WHERE a.DoctorId = d.DoctorId 
                           AND a.AppointmentDateTime >= @StartDate) as TotalAppointments,
                        -- Completed appointments
                        (SELECT COUNT(*) 
                         FROM [Healthcare].[Appointments] a 
                         WHERE a.DoctorId = d.DoctorId 
                           AND a.AppointmentDateTime >= @StartDate 
                           AND a.Status = 'Completed') as CompletedAppointments,
                        -- Cancelled appointments
                        (SELECT COUNT(*) 
                         FROM [Healthcare].[Appointments] a 
                         WHERE a.DoctorId = d.DoctorId 
                           AND a.AppointmentDateTime >= @StartDate 
                           AND a.Status = 'Cancelled') as CancelledAppointments,
                        -- Last appointment date
                        (SELECT MAX(AppointmentDateTime) 
                         FROM [Healthcare].[Appointments] a 
                         WHERE a.DoctorId = d.DoctorId) as LastAppointmentDate
                    FROM [Healthcare].[Doctors] d
                    WHERE EXISTS (SELECT 1 FROM [Healthcare].[Appointments] a 
                                  WHERE a.DoctorId = d.DoctorId 
                                    AND a.AppointmentDateTime >= @StartDate)
                    ORDER BY CompletedAppointments DESC";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", DateTime.Today.AddDays(-days));

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var doctor = new DoctorProductivityReportDTO
                            {
                                DoctorId = reader.GetInt32("DoctorId"),
                                DoctorName = reader["DoctorName"] as string ?? "Unknown",
                                Specialty = reader["Specialty"] as string ?? "General",
                                TotalAppointments = reader.GetInt32("TotalAppointments"),
                                CompletedAppointments = reader.GetInt32("CompletedAppointments"),
                                CancelledAppointments = reader.GetInt32("CancelledAppointments")
                            };

                            // Calculate completion rate
                            if (doctor.TotalAppointments > 0)
                            {
                                doctor.CompletionRate = Math.Round(
                                    (double)doctor.CompletedAppointments / doctor.TotalAppointments * 100, 2);
                            }

                            // Handle nullable LastAppointmentDate
                            if (!reader.IsDBNull("LastAppointmentDate"))
                            {
                                doctor.LastAppointmentDate = reader.GetDateTime("LastAppointmentDate");
                            }

                            Doctors.Add(doctor);
                        }
                    }
                }
            }
        }
    }
}