using AppointEase.Application.Contracts.Identity;
using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models;
using AppointEase.Application.Contracts.Models.DbModels;
using AppointEase.Application.Contracts.ModelsDto;
using AppointEase.Application.Services;
using AppointEase.Data.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointEase.AspNetCore.Controllers
{
    public class AuthController : Controller
    {

        private readonly IPatientService _patientService;

        public AuthController(IPatientService patientService)
        {
           this._patientService = patientService;   
        }


        [HttpPost("CreatePatient")]
        public async Task<IActionResult> CreatePatient([FromBody] PatientRequest patientRequest,string Role)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data provided.");
            }
            ICommon.Role = Role;

            var result = await _patientService.CreatePersonAsync(patientRequest);
            return Ok(result);
        }

        //[HttpPost("Login")]
        //public async Task<IActionResult> Login([FromBody] LoginRequest model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest("Invalid data provided.");
        //    }


        //    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);

        //    if (!result.Succeeded)
        //        return StatusCode(401, "Invalid login attempt.");

        //    var userProfile = _uRepository.GetIdByEmailAndPasswordAsync(model.Email, model.Password);
        //    if (userProfile == null)
        //        return StatusCode(401, "User not found.");

        //    var userPersonalData = _uRepository.GetByIdAsync(userProfile.Id);
        //    var userTask = await _userManager.FindByEmailAsync(model.Email);
        //    if (userTask == null)
        //    {
        //        return BadRequest("User not found.");
        //    }

        //    var roles = await _userManager.GetRolesAsync(userTask);
        //    if (roles.Contains("Admin"))
        //    {
        //        return Ok($"Welcome , you are logged in as an Administrator.");
        //    }
        //    else if (roles.Contains("Manager"))
        //    {
        //        return Ok($"Welcome , you are logged in as a Manager.");
        //    }
        //    else if (roles.Contains("Patient"))
        //    {
        //        return Ok($"Welcome , you are logged in as a Patient.");
        //    }
        //    else
        //    {
        //        return Ok($"Welcome , you are logged in.");
        //    }
        //}
    }
}
