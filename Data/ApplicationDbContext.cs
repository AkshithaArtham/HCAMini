using Microsoft.EntityFrameworkCore;
using HCAMiniEHR.Models;

namespace HCAMiniEHR.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<LabOrder> LabOrders { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set the default schema to Healthcare
            modelBuilder.HasDefaultSchema("Healthcare");

            // Explicitly set schema for each table
            modelBuilder.Entity<Patient>().ToTable("Patients", "Healthcare");
            modelBuilder.Entity<Doctor>().ToTable("Doctors", "Healthcare");
            modelBuilder.Entity<Appointment>().ToTable("Appointments", "Healthcare");
            modelBuilder.Entity<LabOrder>().ToTable("LabOrders", "Healthcare");
            modelBuilder.Entity<AuditLog>().ToTable("AuditLogs", "Healthcare");
        }
    }
}
