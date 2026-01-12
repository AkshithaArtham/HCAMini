using Microsoft.EntityFrameworkCore;
using HCAMiniEHR.Models;

namespace HCAMiniEHR.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<LabOrder> LabOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Healthcare");
            modelBuilder.Entity<Appointment>()
       .ToTable(tb => tb.HasTrigger("trg_Appointment_Update"));
        }
    }
}
