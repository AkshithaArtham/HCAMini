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

            // Configure Appointment relationships
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany()
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure Appointment table for triggers
            // This is needed because the table has INSTEAD OF or AFTER triggers
            modelBuilder.Entity<Appointment>()
                .ToTable(tb => tb.UseSqlOutputClause(false));

            // Add indexes for better performance
            modelBuilder.Entity<Appointment>()
                .HasIndex(a => a.AppointmentDateTime);

            modelBuilder.Entity<Appointment>()
                .HasIndex(a => a.Status);
            // Configure LabOrder relationships
            modelBuilder.Entity<LabOrder>()
                .HasOne(lo => lo.Patient)
                .WithMany()
                .HasForeignKey(lo => lo.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<LabOrder>()
                .HasOne(lo => lo.Appointment)
                .WithMany()
                .HasForeignKey(lo => lo.AppointmentId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure for triggers
            modelBuilder.Entity<LabOrder>()
                .ToTable(tb => tb.UseSqlOutputClause(false));
        }
    }
}
