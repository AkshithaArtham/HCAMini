using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using HCAMiniEHR.Models.DTOs;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HCAMiniEHR.Pages.LabOrders
{
    public class IndexModel : PageModel
    {
        private readonly string _connectionString;

        public List<LabOrderDisplayDTO> LabOrders { get; set; } = new List<LabOrderDisplayDTO>();

        public IndexModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task OnGetAsync()
        {
            await LoadLabOrders();
        }

        private async Task LoadLabOrders()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = @"
                    SELECT 
                        lo.LabOrderId,
                        lo.PatientId,
                        p.Name as PatientName,
                        lo.AppointmentId,
                        lo.TestName,
                        lo.Status,
                        lo.OrderDate,
                        lo.Notes
                    FROM [Healthcare].[LabOrders] lo
                    LEFT JOIN [Healthcare].[Patients] p ON lo.PatientId = p.PatientId
                    ORDER BY lo.OrderDate DESC";

                using (var command = new SqlCommand(sql, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var dto = new LabOrderDisplayDTO
                        {
                            LabOrderId = reader.GetInt32("LabOrderId"),
                            PatientId = reader.GetInt32("PatientId"),
                            PatientName = reader["PatientName"] as string ?? "Unknown",
                            TestName = reader["TestName"] as string ?? "Unknown Test",
                            Status = reader["Status"] as string ?? "Pending",
                            OrderDate = reader["OrderDate"] as DateTime?,
                            Notes = reader["Notes"] as string
                        };

                        // Handle nullable AppointmentId
                        if (!reader.IsDBNull("AppointmentId"))
                        {
                            dto.AppointmentId = reader.GetInt32("AppointmentId");
                        }

                        LabOrders.Add(dto);
                    }
                }
            }
        }
    }
}