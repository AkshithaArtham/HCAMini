using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using System.Collections.Generic;
using System.Linq;

namespace HCAMiniEHR.Services
{
    public class PatientService
    {
        private readonly ApplicationDbContext _context;

        public PatientService(ApplicationDbContext context)
        {
            _context = context;
        }

        // READ
        public List<Patient> GetAll()
        {
            return _context.Patients.ToList();
        }

        // CREATE
        public void Add(Patient patient)
        {
            _context.Patients.Add(patient);
            _context.SaveChanges();
        }
        public Patient? GetById(int id)
        {
            return _context.Patients.Find(id);
        }

        public void Update(Patient patient)
        {
            _context.Patients.Update(patient);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var patient = _context.Patients.Find(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                _context.SaveChanges();
            }
        }

    }
}
