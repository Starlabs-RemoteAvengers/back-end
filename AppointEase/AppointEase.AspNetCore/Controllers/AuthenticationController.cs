using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppointEase.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPatientService _patientService;

        public AuthenticationController(IUserService userService, IPatientService patientService) 
        {
            _userService = userService;
            _patientService = patientService;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
                return BadRequest("Invalid login request");

            var result = await _userService.LogInAsync(loginRequest.Username, loginRequest.Password);

            if (result != null && result is string token)
              {
                  return Ok(new { token });
              }
              else
              {
                  return Unauthorized("Invalid username or password");
              }

        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            try
            {
                var result = await _patientService.ConfirmEmail(token, email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            try
            {
                var result = await _userService.UserForgotPassword(email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody]PasswordRequest passwordRequest)
        {
            try
            {
                var result = await _userService.UserResetPassword(passwordRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordRequest passwordRequest)
        {
            try
            {
                var result = await _userService.UserChangePassword(passwordRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
