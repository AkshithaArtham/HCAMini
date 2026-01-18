using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HCAMiniEHR.Pages
{
    public class IndexModel : PageModel
    {
        private readonly string _connectionString;

        // Stats
        public int TotalPatients { get; set; }
        public int TotalDoctors { get; set; }
        public int TodaysAppointments { get; set; }
        public int PendingLabOrders { get; set; }

        // Recent Activities
        public List<RecentAppointmentDTO> RecentAppointments { get; set; } = new List<RecentAppointmentDTO>();
        public List<RecentLabOrderDTO> RecentLabOrders { get; set; } = new List<RecentLabOrderDTO>();

        public IndexModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task OnGetAsync()
        {
            await LoadStatistics();
            await LoadRecentAppointments();
            await LoadRecentLabOrders();
        }

        private async Task LoadStatistics()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Total Patients
                var patientSql = "SELECT COUNT(*) FROM [Healthcare].[Patients]";
                using (var cmd = new SqlCommand(patientSql, connection))
                {
                    TotalPatients = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                }

                // Total Doctors
                var doctorSql = "SELECT COUNT(*) FROM [Healthcare].[Doctors]";
                using (var cmd = new SqlCommand(doctorSql, connection))
                {
                    TotalDoctors = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                }

                // Today's Appointments
                var todaySql = @"
                    SELECT COUNT(*) 
                    FROM [Healthcare].[Appointments] 
                    WHERE CONVERT(DATE, AppointmentDateTime) = CONVERT(DATE, GETDATE()) 
                      AND Status != 'Cancelled'";
                using (var cmd = new SqlCommand(todaySql, connection))
                {
                    TodaysAppointments = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                }

                // Pending Lab Orders
                var pendingSql = "SELECT COUNT(*) FROM [Healthcare].[LabOrders] WHERE Status = 'Pending'";
                using (var cmd = new SqlCommand(pendingSql, connection))
                {
                    PendingLabOrders = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                }
            }
        }

        private async Task LoadRecentAppointments()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = @"
                    SELECT TOP 5 
                        a.AppointmentId,
                        p.Name as PatientName,
                        a.AppointmentDateTime,
                        a.Status
                    FROM [Healthcare].[Appointments] a
                    JOIN [Healthcare].[Patients] p ON a.PatientId = p.PatientId
                    ORDER BY a.AppointmentDateTime DESC";

                using (var command = new SqlCommand(sql, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        RecentAppointments.Add(new RecentAppointmentDTO
                        {
                            AppointmentId = reader.GetInt32("AppointmentId"),
                            PatientName = reader["PatientName"] as string ?? "Unknown",
                            AppointmentDate = reader.GetDateTime("AppointmentDateTime"),
                            Status = reader["Status"] as string ?? "Scheduled"
                        });
                    }
                }
            }
        }

        private async Task LoadRecentLabOrders()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = @"
                    SELECT TOP 5 
                        lo.LabOrderId,
                        p.Name as PatientName,
                        lo.TestName,
                        lo.Status,
                        lo.OrderDate
                    FROM [Healthcare].[LabOrders] lo
                    LEFT JOIN [Healthcare].[Patients] p ON lo.PatientId = p.PatientId
                    ORDER BY lo.OrderDate DESC";

                using (var command = new SqlCommand(sql, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        RecentLabOrders.Add(new RecentLabOrderDTO
                        {
                            LabOrderId = reader.GetInt32("LabOrderId"),
                            PatientName = reader["PatientName"] as string ?? "Unknown",
                            TestName = reader["TestName"] as string ?? "Unknown Test",
                            Status = reader["Status"] as string ?? "Pending",
                            OrderDate = reader["OrderDate"] as DateTime?
                        });
                    }
                }
            }
        }
    }

    // DTOs for dashboard
    public class RecentAppointmentDTO
    {
        public int AppointmentId { get; set; }
        public string PatientName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
    }

    public class RecentLabOrderDTO
    {
        public int LabOrderId { get; set; }
        public string PatientName { get; set; }
        public string TestName { get; set; }
        public string Status { get; set; }
        public DateTime? OrderDate { get; set; }
    }
}