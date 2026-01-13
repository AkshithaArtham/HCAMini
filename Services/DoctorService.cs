using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using Microsoft.EntityFrameworkCore;

namespace HCAMiniEHR.Services
{
    public class DoctorService
    {
        private readonly ApplicationDbContext _context;

        public DoctorService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create Doctor
        public async Task AddDoctorAsync(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
        }

        // Get all Doctors
        public async Task<List<Doctor>> GetDoctorsAsync()
        {
            return await _context.Doctors.ToListAsync();
        }

        // Get a single Doctor by ID
        public async Task<Doctor> GetDoctorByIdAsync(int id)
        {
            return await _context.Doctors.FindAsync(id);
        }

        // Update Doctor
        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();
        }

        // Delete Doctor
        public async Task DeleteDoctorAsync(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
            }
        }
    }
}
