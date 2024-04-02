using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointEase.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        [HttpPost]
        public async Task<IActionResult>CreateAppointment(AppointmentRequest request)
        {
            var result = await _appointmentService.CreateAppointment(request);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAppointments()
        {
            var result = await _appointmentService.GetAllAppointments();
            return Ok(result);
        }
        [HttpPost("AcceptAppointment")]
        public async Task<IActionResult> AcceptAppointment(string id)
        {
            var result = await _appointmentService.AcceptAppointment(id);
            return Ok(result);
        }
        [HttpPost("DeclineAppointment")]
        public async Task<IActionResult> DeclineAppointment(string id)
        {
            var result = await _appointmentService.DeclineAppointment(id);
            return Ok(result);
        }
        [HttpPut("DoctorCancelAppointment")]
        public async Task<IActionResult> DoctorCanceclAppointment(string id)
        {
            var result = await _appointmentService.DoctorCancelAppointment(id);
            return Ok(result);
        }
        [HttpPut("PatientCancelAppointment")]
        public async Task<IActionResult> PatientCanceclAppointment(string id)
        {
            var result = await _appointmentService.PatientCancelAppointment(id);
            return Ok(result);
        }

        [HttpPost("PatientCancelAppointmentPostMethod")]
        public async Task<IActionResult> PatientCancelAppointmentPostMethod(string id)
        {
            var result = await _appointmentService.PatientCancelAppointmentPostMethod(id);
            return Ok(result);
        }
    }
}
