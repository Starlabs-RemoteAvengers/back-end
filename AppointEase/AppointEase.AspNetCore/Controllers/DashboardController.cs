using AppointEase.Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointEase.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            this._dashboardService = dashboardService;
        }

        [HttpGet("Patient-Dashboard")]
        public async Task<object> PatientDashboard([FromQuery]string patientId)
        {
            var result = await _dashboardService.GetDashboardForPatient(patientId);
            return result;
        }

    }
}
