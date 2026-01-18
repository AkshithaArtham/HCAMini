using Microsoft.Data.SqlClient;
using HCAMiniEHR.Models.DTOs;
using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace HCAMiniEHR.Services
{
    public class LabOrderService
    {
        private readonly string _connectionString;

        public LabOrderService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Create Lab Order using Stored Procedure
        public async Task<int> CreateLabOrderAsync(CreateLabOrderDTO dto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("[Healthcare].[CreateLabOrder]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parameters from DTO
                    command.Parameters.AddWithValue("@PatientId", dto.PatientId);
                    command.Parameters.AddWithValue("@AppointmentId", dto.AppointmentId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@TestName", dto.TestName);
                    command.Parameters.AddWithValue("@Notes", dto.Notes ?? (object)DBNull.Value);

                    var result = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(result);
                }
            }
        }

        // Update Lab Order Status
        public async Task<bool> UpdateLabOrderStatusAsync(int labOrderId, string status, string resultNotes = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("[Healthcare].[UpdateLabOrderStatus]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parameters
                    command.Parameters.AddWithValue("@LabOrderId", labOrderId);
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@ResultNotes", resultNotes ?? (object)DBNull.Value);

                    var result = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(result) == 1;
                }
            }
        }
    }
}