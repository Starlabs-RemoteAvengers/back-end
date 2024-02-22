using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Data.Contracts.Identity;
using AppointEase.Data.Data;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointEase.AspNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IUserService _userService;
        private readonly IValidator<PatientRequest> _patientValidator;
        public AccountController(IPatientService patientService,IValidator<PatientRequest> patientValidator, IUserService userService)
        {
            this._patientService = patientService;
            this._patientValidator = patientValidator;
            this._userService = userService;
        }

        [HttpPost("CreatePatient")]
        public async Task<IActionResult> CreatePatient([FromBody] PatientRequest patientRequest)
        {
            _patientValidator.ValidateAndThrow(patientRequest);

            var result = await _patientService.CreatePatitentAsync(patientRequest);
            return Ok(result);
        }

        [HttpPut("UpdatePatient/{personId}")]
        public async Task<IActionResult> UpdatePatient(int personId, [FromBody] PatientRequest patientRequest)
        {
            _patientValidator.ValidateAndThrow(patientRequest);

            var result = await _patientService.UpdatePatitent(personId, patientRequest);

            return Ok(result);
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
                return BadRequest("Invalid login request");

            var result = await _userService.LogInAsync(loginRequest.Username, loginRequest.Password);

            if (result.Success)
              return Ok();
            
            return Unauthorized("Invalid username or password");
            
        }

    }
}

