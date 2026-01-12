using HCAMiniEHR.Models;
using HCAMiniEHR.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HCAMiniEHR.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly AppointmentService _appointmentService;
        private readonly PatientService _patientService;

        public AppointmentController(
            AppointmentService appointmentService,
            PatientService patientService)
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
        }

        // LIST
        public IActionResult Index()
        {
            var appointments = _appointmentService.GetAll();
            return View(appointments);
        }

        // CREATE FORM
        public IActionResult Create()
        {
            ViewBag.Patients = new SelectList(
                _patientService.GetAll(),
                "PatientId",
                "FullName"
            );

            return View();
        }

        // CREATE POST
        [HttpPost]
        public IActionResult Create(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Patients = new SelectList(
                    _patientService.GetAll(),
                    "PatientId",
                    "FullName"
                );
                return View(appointment);
            }

            // USE STORED PROCEDURE HERE
            _appointmentService.AddUsingStoredProcedure(appointment);

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            var appointment = _appointmentService.GetById(id);
            if (appointment == null)
                return NotFound();

            ViewBag.Patients = new SelectList(
                _patientService.GetAll(),
                "PatientId",
                "FullName",
                appointment.PatientId
            );

            return View(appointment);
        }
        [HttpPost]
        public IActionResult Edit(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Patients = new SelectList(
                    _patientService.GetAll(),
                    "PatientId",
                    "FullName",
                    appointment.PatientId
                );
                return View(appointment);
            }

            _appointmentService.Update(appointment);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var appointment = _appointmentService.GetById(id);
            if (appointment == null)
                return NotFound();

            return View(appointment);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _appointmentService.Delete(id);
            return RedirectToAction(nameof(Index));
        }





    }
}
