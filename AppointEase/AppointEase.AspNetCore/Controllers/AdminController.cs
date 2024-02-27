using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Request;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointEase.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IValidator<AdminRequest> _adminValidator;
        public AdminController(IAdminService adminService, IValidator<AdminRequest> adminValidator, IUserService userService)
        {
            _adminService = adminService;
            _adminValidator = adminValidator;
        }

        [HttpPost("CreateAdmin")]
        public async Task<IActionResult> CreateAdmin([FromBody] AdminRequest adminRequest)
        {
            _adminValidator.ValidateAndThrow(adminRequest);

            var result = await _adminService.CreateAdminAsync(adminRequest);
            return Ok(result);
        }

        [HttpPut("UpdateAdmin/{personId}")]
        public async Task<IActionResult> UpdateAdmin(string personId, [FromBody] AdminRequest adminRequest)
        {
            _adminValidator.ValidateAndThrow(adminRequest);

            var result = await _adminService.UpdateAdmin(personId, adminRequest);

            return Ok(result);
        }

        [HttpGet("GetAdminById")]
        public async Task<IActionResult> GetAdminById(string adminId)
        {
            var result = await _adminService.GetAdmin(adminId);
            return Ok(result);
        }

        [HttpGet("GetAllAdmin")]
        public async Task<ActionResult<IEnumerable<AdminRequest>>> GetAllAdmin()
        {
            var result = await _adminService.GetAllAdmins();

            return Ok(result);
        }

        [HttpDelete("DeleteAdmin")]
        public async Task<IActionResult> DeleteAdmin(string adminId)
        {
            var result = await _adminService.DeleteAdmin(adminId);
            return Ok(result);
        }
    }
}
