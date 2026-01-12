using HCAMiniEHR.Models;
using HCAMiniEHR.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HCAMiniEHR.Controllers
{
    public class LabOrderController : Controller
    {
        private readonly LabOrderService _labOrderService;
        private readonly AppointmentService _appointmentService;

        public LabOrderController(
            LabOrderService labOrderService,
            AppointmentService appointmentService)
        {
            _labOrderService = labOrderService;
            _appointmentService = appointmentService;
        }

        // LIST
        public IActionResult Index()
        {
            var labOrders = _labOrderService.GetAll();
            return View(labOrders);
        }

        // CREATE FORM
        public IActionResult Create()
        {
            ViewBag.Appointments = new SelectList(
                _appointmentService.GetAll(),
                "AppointmentId",
                "AppointmentId"
            );

            return View();
        }

        // CREATE POST
        [HttpPost]
        public IActionResult Create(LabOrder labOrder)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Appointments = new SelectList(
                    _appointmentService.GetAll(),
                    "AppointmentId",
                    "AppointmentId"
                );
                return View(labOrder);
            }

            _labOrderService.Add(labOrder);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            var labOrder = _labOrderService.GetById(id);
            if (labOrder == null)
                return NotFound();

            ViewBag.Appointments = new SelectList(
                _appointmentService.GetAll(),
                "AppointmentId",
                "AppointmentId",
                labOrder.AppointmentId
            );

            return View(labOrder);
        }
        [HttpPost]
        public IActionResult Edit(LabOrder labOrder)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Appointments = new SelectList(
                    _appointmentService.GetAll(),
                    "AppointmentId",
                    "AppointmentId",
                    labOrder.AppointmentId
                );
                return View(labOrder);
            }

            _labOrderService.Update(labOrder);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var labOrder = _labOrderService.GetById(id);
            if (labOrder == null)
                return NotFound();

            return View(labOrder);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _labOrderService.Delete(id);
            return RedirectToAction(nameof(Index));
        }



    }
}
