<<<<<<< HEAD
using HCAMiniEHR.Models;
using HCAMiniEHR.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HCAMiniEHR.Pages.Doctors
{
    public class CreateModel : PageModel
    {
        private readonly DoctorService _doctorService;

        public CreateModel(DoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [BindProperty]
        public Doctor Doctor { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _doctorService.AddDoctorAsync(Doctor);
            return RedirectToPage("Index");
        }

    }
}
=======
using HCAMiniEHR.Models;
using HCAMiniEHR.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HCAMiniEHR.Pages.Doctors
{
    public class CreateModel : PageModel
    {
        private readonly DoctorService _doctorService;

        public CreateModel(DoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [BindProperty]
        public Doctor Doctor { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _doctorService.AddDoctorAsync(Doctor);
            return RedirectToPage("Index");
        }

    }
}
>>>>>>> de0c1979792b9fba70a0d3608ff20cf61cb6a43b
