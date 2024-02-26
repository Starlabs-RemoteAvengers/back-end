using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Request;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointEase.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IValidator<DoctorRequest> _doctorValidator;
        public DoctorController(IDoctorService doctorService, IValidator<DoctorRequest> doctorValidator)
        {
            _doctorService = doctorService;
            _doctorValidator = doctorValidator;
        }

        [HttpPost("CreateDoctor")]
        public async Task<IActionResult> CreateDoctor([FromBody] DoctorRequest doctorRequest)
        {
            _doctorValidator.ValidateAndThrow(doctorRequest);

            var result = await _doctorService.CreateDoctorAsync(doctorRequest);
            return Ok(result);
        }



        [HttpPut("UpdateDoctor/{personId}")]
        public async Task<IActionResult> UpdateDoctor(string personId, [FromBody] DoctorRequest doctorRequest)
        {
            _doctorValidator.ValidateAndThrow(doctorRequest);

            var result = await _doctorService.UpdateDoctor(personId, doctorRequest);

            return Ok(result);
        }

        [HttpGet("GetDoctorById")]
        public async Task<IActionResult> GetDoctorById(string doctorId)
        {
            var result = await _doctorService.GetDoctor(doctorId);
            return Ok(result);
        }

        [HttpGet("GetAllDoctors")]
        public async Task<ActionResult<IEnumerable<DoctorRequest>>> GetAllDoctors()
        {
            var result = await _doctorService.GetAllDoctors();

            return Ok(result);
        }

        [HttpDelete("DeleteDoctor")]
        public async Task<IActionResult> DeleteClinc(string doctorId)
        {
            var result = await _doctorService.DeleteDoctor(doctorId);
            return Ok(result);
        }
    }
}
