using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Data.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Data.Contracts.Identity;
using AppointEase.Data.Contracts.Models;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;
using AppointEase.Application.Contracts.Models.EmailConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace AppointEase.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IRepository<Patient> _patientRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IApplicationExtensions _common;
        private readonly IOperationResult _operationResult;
        private readonly IEmailServices _email;
        private readonly IConfiguration _configuration;
        public PatientService( IConfiguration _configuration, IEmailServices _email,IRepository<Patient> patientRepository, UserManager<ApplicationUser> userManager, IMapper mapper, IApplicationExtensions common, IOperationResult operationResult)
        {
            _patientRepository = patientRepository;
            _userManager = userManager;
            _mapper = mapper;
            _common = common;
            _operationResult = operationResult;
            this._configuration = _configuration;
            this._email = _email;
        }

        public async Task<OperationResult> CreatePatientAsync(PatientRequest patientRequest)
        {
            try
            {

                var patientExists = await CheckIfPatientExists(patientRequest.UserName,patientRequest.Email, patientRequest.PersonalNumber, null);

                if (patientExists != null)
                {
                    return _operationResult.ErrorResult("Failed to create patient:", new[] { "This patient exists, please try again!" });
                }

                var user = _mapper.Map<Patient>(patientRequest);

                var result = await _userManager.CreateAsync(user, patientRequest.Password);

                if (!result.Succeeded)
                {
                    return _operationResult.ErrorResult($"Failed to create user:", result.Errors.Select(e => e.Description));
                }

                await _userManager.AddToRoleAsync(user, user.Role);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                _common.SendEmailConfirmation(token, user.Email);
                _common.AddInformationMessage("Patient created successfully!");

                return _operationResult.SuccessResult("Patient created successfully!");
            }
            catch (FluentValidation.ValidationException validationException)
            {
                return _operationResult.ErrorResult("Failed to create patient: Validation error", validationException.Errors.Select(e => e.ErrorMessage));
            }
            catch (Exception ex)
            {
                _common.AddErrorMessage($"Error creating patient: {ex.Message}");
                return _operationResult.ErrorResult($"Failed to create user:", new[] { ex.Message });
            }
        }


        private async Task<Patient> CheckIfPatientExists(string username, string email, int personalNumber, string currentUserId)
        {
            var patients = await GetAllPatients();
            var patientRequest = patients.FirstOrDefault(p =>
                p != null && (p.UserName == username|| p.Email == email || p.PersonalNumber == personalNumber) && p.Id != currentUserId);

            if (patientRequest != null)
            {
                return _mapper.Map<Patient>(patientRequest);
            }

            return null;
        }

        public async Task<OperationResult> DeletePatient(string patientId)
        {
            try
            {
                var patient = await _userManager.FindByIdAsync(patientId);

                if (patient == null)
                {
                    return _operationResult.ErrorResult($"Patient not found with ID: {patientId}", new[] { "Patient not found." });
                }

                var deleteResult = await _userManager.DeleteAsync(patient);

                if (!deleteResult.Succeeded)
                {
                    return _operationResult.ErrorResult($"Failed to delete patient:", deleteResult.Errors.Select(e => e.Description));
                }

                _common.AddInformationMessage("Patient deleted successfully!");

                return _operationResult.SuccessResult("Patient deleted successfully!");
            }
            catch (Exception ex)
            {
                _common.AddErrorMessage($"Error deleting patient: {ex.Message}");
                return _operationResult.ErrorResult($"Failed to delete patient:", new[] { ex.Message });
            }
        }
        public async Task<IEnumerable<PatientRequest>> GetAllPatients()
        {
            var patients = await _patientRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PatientRequest>>(patients);
        }
        public async Task<PatientRequest> GetPatient(string patientId)
        {
            var patient = await _userManager.FindByIdAsync(patientId);

            if (patient == null)
            {
                return null;
            }

            return _mapper.Map<PatientRequest>(patient);
        }
        public async Task<OperationResult> UpdatePatient(string userId, PatientRequest patientRequest)
        {
            try
            {
                var existingPatient = await _userManager.FindByIdAsync(userId);

                if (existingPatient == null)
                {
                    return _operationResult.ErrorResult("Failed to update patient:", new[] { "Patient not found!" });
                }

                var patientExists = await CheckIfPatientExists(patientRequest.UserName,patientRequest.Email, patientRequest.PersonalNumber, userId);

                if (patientExists != null)
                {
                    return _operationResult.ErrorResult("Failed to update patient:", new[] { "This email or personal number or UserName is already associated with another patient!" });
                }

                if (!string.IsNullOrEmpty(patientRequest.Password))
                {
                    var passwordHasher = new PasswordHasher<ApplicationUser>();
                    var hashedPassword = passwordHasher.HashPassword(existingPatient, patientRequest.Password);
                    existingPatient.PasswordHash = hashedPassword;
                }

                UpdatePatientProperties(existingPatient, patientRequest);

                var result = await _userManager.UpdateAsync(existingPatient);

                if (!result.Succeeded)
                {
                    return _operationResult.ErrorResult($"Failed to update patient:", result.Errors.Select(e => e.Description));
                }

                _common.AddInformationMessage("Patient updated successfully!");

                return _operationResult.SuccessResult("Patient updated successfully!");
            }
            catch (FluentValidation.ValidationException validationException)
            {
                return _operationResult.ErrorResult("Failed to update patient: Validation error", validationException.Errors.Select(e => e.ErrorMessage));
            }
            catch (Exception ex)
            {
                _common.AddErrorMessage($"Error updating patient: {ex.Message}");
                return _operationResult.ErrorResult($"Failed to update patient:", new[] { ex.Message });
            }
        }
        private void UpdatePatientProperties(ApplicationUser existingPatient, PatientRequest patientRequest)
        {
            // Use AutoMapper to map properties from PatientRequest to ApplicationUser
            _mapper.Map(patientRequest, existingPatient);

            if (existingPatient is Patient patientToUpdate)
            {
                // Additional mapping for properties specific to the Patient class
                _mapper.Map(patientRequest, patientToUpdate);
            }
        }
        public async Task<OperationResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new Exception("User with this email doesn't excist!");

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return _operationResult.SuccessResult("Email verified successfully!");

            }

            return _operationResult.ErrorResult($"Failed to verified user:", new[] { "" });
        }
    }
}