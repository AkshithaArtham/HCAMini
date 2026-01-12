using System.Collections.Generic;
using System.Linq;
using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using Microsoft.EntityFrameworkCore;

namespace HCAMiniEHR.Services
{
    public class LabOrderService
    {
        private readonly ApplicationDbContext _context;

        public LabOrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<LabOrder> GetAll()
        {
            return _context.LabOrders
                           .Include(l => l.Appointment)
                           .ThenInclude(a => a.Patient)
                           .ToList();
        }

        public void Add(LabOrder labOrder)
        {
            _context.LabOrders.Add(labOrder);
            _context.SaveChanges();
        }
        public LabOrder? GetById(int id)
        {
            return _context.LabOrders
                .Include(l => l.Appointment)
                .ThenInclude(a => a.Patient)
                .FirstOrDefault(l => l.LabOrderId == id);
        }

        public void Update(LabOrder labOrder)
        {
            _context.LabOrders.Update(labOrder);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var labOrder = _context.LabOrders.Find(id);
            if (labOrder != null)
            {
                _context.LabOrders.Remove(labOrder);
                _context.SaveChanges();
            }
        }

    }
}
