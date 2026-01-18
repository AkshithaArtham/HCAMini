using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using HCAMiniEHR.Models.DTOs;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HCAMiniEHR.Pages.LabOrders
{
    public class DetailsModel : PageModel
    {
        private readonly string _connectionString;

        public LabOrderDetailDTO LabOrder { get; set; }
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();

        public DetailsModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await LoadLabOrderDetails(id.Value);

            if (LabOrder == null)
            {
                return NotFound();
            }

            await LoadAuditLogs(id.Value);

            return Page();
        }

        private async Task LoadLabOrderDetails(int labOrderId)
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
                        lo.ResultDate,
                        lo.Notes,
                        lo.ResultNotes,
                        a.AppointmentDateTime,
                        d.Name as DoctorName
                    FROM [Healthcare].[LabOrders] lo
                    LEFT JOIN [Healthcare].[Patients] p ON lo.PatientId = p.PatientId
                    LEFT JOIN [Healthcare].[Appointments] a ON lo.AppointmentId = a.AppointmentId
                    LEFT JOIN [Healthcare].[Doctors] d ON a.DoctorId = d.DoctorId
                    WHERE lo.LabOrderId = @LabOrderId";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LabOrderId", labOrderId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            LabOrder = new LabOrderDetailDTO
                            {
                                LabOrderId = reader.GetInt32("LabOrderId"),
                                PatientId = reader.GetInt32("PatientId"),
                                PatientName = reader["PatientName"] as string ?? "Unknown",
                                TestName = reader["TestName"] as string ?? "Unknown Test",
                                Status = reader["Status"] as string ?? "Pending",
                                OrderDate = reader["OrderDate"] as DateTime?,
                                ResultDate = reader["ResultDate"] as DateTime?,
                                Notes = reader["Notes"] as string,
                                ResultNotes = reader["ResultNotes"] as string
                            };

                            // Handle nullable fields
                            if (!reader.IsDBNull("AppointmentId"))
                            {
                                LabOrder.AppointmentId = reader.GetInt32("AppointmentId");
                            }

                            if (!reader.IsDBNull("AppointmentDateTime"))
                            {
                                LabOrder.AppointmentDate = reader.GetDateTime("AppointmentDateTime");
                            }

                            LabOrder.DoctorName = reader["DoctorName"] as string;
                        }
                    }
                }
            }
        }

        private async Task LoadAuditLogs(int labOrderId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = @"
                    SELECT 
                        AuditLogId,
                        Action,
                        OldValues,
                        NewValues,
                        ChangedAt,
                        ChangedBy
                    FROM [Healthcare].[AuditLogs]
                    WHERE TableName = 'LabOrders' AND RecordId = @LabOrderId
                    ORDER BY ChangedAt DESC";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LabOrderId", labOrderId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            AuditLogs.Add(new AuditLogDTO
                            {
                                AuditLogId = reader.GetInt32("AuditLogId"),
                                Action = reader["Action"] as string,
                                OldValues = reader["OldValues"] as string,
                                NewValues = reader["NewValues"] as string,
                                ChangedAt = reader.GetDateTime("ChangedAt"),
                                ChangedBy = reader["ChangedBy"] as string
                            });
                        }
                    }
                }
            }
        }
    }
}