using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HCAMiniEHR.Migrations
{
    /// <inheritdoc />
    public partial class CreateAppointmentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Doctors_DoctorId",
                schema: "Healthcare",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                schema: "Healthcare",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "AppointmentDate",
                schema: "Healthcare",
                table: "Appointments",
                newName: "CreatedDate");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "Healthcare",
                table: "Appointments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                schema: "Healthcare",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentDateTime",
                schema: "Healthcare",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Healthcare",
                table: "Appointments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                schema: "Healthcare",
                table: "Appointments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AppointmentDateTime",
                schema: "Healthcare",
                table: "Appointments",
                column: "AppointmentDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Status",
                schema: "Healthcare",
                table: "Appointments",
                column: "Status");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Doctors_DoctorId",
                schema: "Healthcare",
                table: "Appointments",
                column: "DoctorId",
                principalSchema: "Healthcare",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                schema: "Healthcare",
                table: "Appointments",
                column: "PatientId",
                principalSchema: "Healthcare",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Doctors_DoctorId",
                schema: "Healthcare",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                schema: "Healthcare",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_AppointmentDateTime",
                schema: "Healthcare",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_Status",
                schema: "Healthcare",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AppointmentDateTime",
                schema: "Healthcare",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Healthcare",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Reason",
                schema: "Healthcare",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                schema: "Healthcare",
                table: "Appointments",
                newName: "AppointmentDate");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "Healthcare",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                schema: "Healthcare",
                table: "Appointments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Doctors_DoctorId",
                schema: "Healthcare",
                table: "Appointments",
                column: "DoctorId",
                principalSchema: "Healthcare",
                principalTable: "Doctors",
                principalColumn: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                schema: "Healthcare",
                table: "Appointments",
                column: "PatientId",
                principalSchema: "Healthcare",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
