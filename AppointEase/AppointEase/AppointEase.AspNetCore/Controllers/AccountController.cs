using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Request;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AppointEase.AspNetCore.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class AccountController : Controller
    {
        private readonly IPatientService _patientService;
        IValidator<PatientRequest> _patientValidator;

        public AccountController(IPatientService patientService, IValidator<PatientRequest> patientValidator)
        {
           this._patientService = patientService;
            this._patientValidator = patientValidator;
        }

        [HttpPost("CreatePatient")]
        public async Task<IActionResult> CreatePatient([FromBody] PatientRequest patientRequest)
        {
            _patientValidator.ValidateAndThrow(patientRequest);

            var result = await _patientService.CreatePatitentAsync(patientRequest);
            return Ok(result);
        }
    }
}
