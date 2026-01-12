using HCAMiniEHR.Models;
using HCAMiniEHR.Services;
using Microsoft.AspNetCore.Mvc;

namespace HCAMiniEHR.Controllers
{
    public class PatientController : Controller
    {
        private readonly PatientService _service;

        public PatientController(PatientService service)
        {
            _service = service;
        }

        // GET: /Patient/Index
        public IActionResult Index()
        {
            var patients = _service.GetAll();
            return View(patients);
        }

        // GET: /Patient/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Patient/Create
        [HttpPost]
        public IActionResult Create(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            _service.Add(patient);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            var patient = _service.GetById(id);
            if (patient == null)
                return NotFound();

            return View(patient);
        }
        [HttpPost]
        public IActionResult Edit(Patient patient)
        {
            if (!ModelState.IsValid)
                return View(patient);

            _service.Update(patient);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var patient = _service.GetById(id);
            if (patient == null)
                return NotFound();

            return View(patient);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
