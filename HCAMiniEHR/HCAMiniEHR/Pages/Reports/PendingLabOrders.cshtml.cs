using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Models.DTOs;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HCAMiniEHR.Pages.Reports
{
    public class PendingLabOrdersModel : PageModel
    {
        private readonly string _connectionString;

        public List<PendingLabOrderReportDTO> PendingLabOrders { get; set; } = new List<PendingLabOrderReportDTO>();
        public int TotalPendingOrders { get; set; }

        public PendingLabOrdersModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task OnGetAsync()
        {
            await LoadPendingLabOrders();
            TotalPendingOrders = PendingLabOrders.Count;
        }

        private async Task LoadPendingLabOrders()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

            var sql = @"
            SELECT 
                lo.LabOrderId,
                p.Name as PatientName,
                lo.TestName,
                lo.OrderDate,
                DATEDIFF(day, ISNULL(lo.OrderDate, GETDATE()), GETDATE()) as DaysPending,
                lo.Notes
            FROM [Healthcare].[LabOrders] lo
            LEFT JOIN [Healthcare].[Patients] p ON lo.PatientId = p.PatientId
            WHERE lo.Status = 'Pending' OR lo.Status IS NULL
            ORDER BY ISNULL(lo.OrderDate, GETDATE()) ASC";

                using (var command = new SqlCommand(sql, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        PendingLabOrders.Add(new PendingLabOrderReportDTO
                        {
                            LabOrderId = reader.GetInt32("LabOrderId"),
                            PatientName = reader["PatientName"] as string ?? "Unknown",
                            TestName = reader["TestName"] as string ?? "Unknown Test",
                            OrderDate = reader.GetDateTime("OrderDate"),
                            DaysPending = reader.GetInt32("DaysPending"),
                            Notes = reader["Notes"] as string
                        });
                    }
                }
            }
        }
    }
}