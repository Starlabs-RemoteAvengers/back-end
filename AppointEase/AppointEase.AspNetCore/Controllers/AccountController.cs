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
        private readonly IValidator<PatientRequest> _patientValidator;
        private readonly AppointEaseContext _dbContext; // Inject the AppointEaseContext here
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IPatientService patientService,IValidator<PatientRequest> patientValidator,AppointEaseContext dbContext,UserManager<ApplicationUser> userManager)
        {
            this._patientService = patientService;
            this._patientValidator = patientValidator;
            this._dbContext = dbContext;
            this._userManager = userManager;
        }

        [HttpPost("CreatePatient")]
        public async Task<IActionResult> CreatePatient([FromBody] PatientRequest patientRequest)
        {
            _patientValidator.ValidateAndThrow(patientRequest);

            var result = await _patientService.CreatePatitentAsync(patientRequest);
            return Ok(result);
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Invalid login request");
            }

            // Use UserManager to find the user by username
            var user = await _userManager.FindByNameAsync(loginRequest.Username);

            if (user != null)
            {
                // Use UserManager to check the password
                var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

                if (isPasswordValid)
                {
                    // User exists, password is correct
                    // Retrieve the user's role
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var userRole = userRoles.FirstOrDefault(); // Assuming a user has only one role

                    // You can return additional information like Role, etc.
                    return Ok(new { Role = userRole, Username = loginRequest.Username });
                }
            }

            // Invalid username or password
            return Unauthorized("Invalid username or password");
        }

    }
}

