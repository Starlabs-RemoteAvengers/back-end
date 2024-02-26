using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Request;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointEase.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicController : ControllerBase
    {
        private readonly IClinicService _clinicService;
        private readonly IValidator<ClinicRequest> _clinicValidator;
        public ClinicController(IClinicService clinicService, IValidator<ClinicRequest> clinicValidator)
        {
            _clinicService = clinicService;
            _clinicValidator = clinicValidator;
        }

        [HttpPost("CreateClinic")]
        public async Task<IActionResult> CreateClinic([FromBody] ClinicRequest clinicRequest)
        {
            _clinicValidator.ValidateAndThrow(clinicRequest);

            var result = await _clinicService.CreateClinicAsync(clinicRequest);
            return Ok(result);
        }



        [HttpPut("UpdateClinic/{personId}")]
        public async Task<IActionResult> UpdateClinic(string personId, [FromBody] ClinicRequest clinicRequest)
        {
            _clinicValidator.ValidateAndThrow(clinicRequest);

            var result = await _clinicService.UpdateClinic(personId, clinicRequest);

            return Ok(result);
        }

        [HttpGet("GetClinicById")]
        public async Task<IActionResult> GetClinicById(string clinicId)
        {
            var result = await _clinicService.GetClinic(clinicId);
            return Ok(result);
        }

        [HttpGet("GetAllClinics")]
        public async Task<ActionResult<IEnumerable<ClinicRequest>>> GetAllClinics()
        {
            var result = await _clinicService.GetAllClinics();

            return Ok(result);
        }

        [HttpDelete("DeleteClinic")]
        public async Task<IActionResult> DeleteClinc(string clinicId)
        {
            var result = await _clinicService.DeleteClinic(clinicId);
            return Ok(result);
        }
    }
}
