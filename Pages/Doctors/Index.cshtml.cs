using HCAMiniEHR.Models;
using HCAMiniEHR.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HCAMiniEHR.Pages.Doctors
{
    public class IndexModel : PageModel
    {
        private readonly DoctorService _doctorService;

        public IndexModel(DoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        public List<Doctor> Doctors { get; set; }

        public async Task OnGetAsync()
        {
            Doctors = await _doctorService.GetDoctorsAsync();
        }
    }
}
