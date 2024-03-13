using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Data.Contracts.Identity;
using AppointEase.Data.Contracts.Interfaces;
using AppointEase.Data.Contracts.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointEase.Application.Services
{
    public class ClinicService : IClinicService
    {
        private readonly IRepository<Clinic> _clinicRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IApplicationExtensions _common;
        private readonly IOperationResult _operationResult;

        public ClinicService(IRepository<Clinic> clinicRepository, UserManager<ApplicationUser> userManager, IMapper mapper, IApplicationExtensions common, IOperationResult operationResult)
        {
            _clinicRepository = clinicRepository;
            _userManager = userManager;
            _mapper = mapper;
            _common = common;
            _operationResult = operationResult;
        }

        public async Task<OperationResult> CreateClinicAsync(ClinicRequest clinicRequest)
        {
            try
            {
                // You can add validation logic here if needed

                var clinicExists = await CheckIfClinicExists(clinicRequest.Email, null);

                if (clinicExists != null)
                {
                    return _operationResult.ErrorResult("Failed to create clinic:", new[] { "This clinic exists, please try again!" });
                }

                var user = _mapper.Map<Clinic>(clinicRequest);

                var result = await _userManager.CreateAsync(user, clinicRequest.Password);

                if (!result.Succeeded)
                {
                    return _operationResult.ErrorResult($"Failed to create user:", result.Errors.Select(e => e.Description));
                }

                await _userManager.AddToRoleAsync(user, user.Role);


                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                _common.SendEmailConfirmation(token, user.Email);
                _common.AddInformationMessage("Clinic created successfully!");

                return _operationResult.SuccessResult("Clinic created successfully!");
            }
            catch (FluentValidation.ValidationException validationException)
            {
                return _operationResult.ErrorResult("Failed to create clinic: Validation error", validationException.Errors.Select(e => e.ErrorMessage));
            }
            catch (Exception ex)
            {
                _common.AddErrorMessage($"Error creating clinic: {ex.Message}");
                return _operationResult.ErrorResult($"Failed to create user:", new[] { ex.Message });
            }
        }

        private async Task<Clinic> CheckIfClinicExists(string email, string currentUserId)
        {
            var clinics = await GetAllClinics();

            var clinicRequest = clinics.FirstOrDefault(c =>
                c != null && (c.Email == email) && c.Id != currentUserId);

            if (clinicRequest != null)
            {
                return _mapper.Map<Clinic>(clinicRequest);
            }

            return null;
        }

        public async Task<OperationResult> DeleteClinic(string clinicaID)
        {
            try
            {
                var clinic = await _userManager.FindByIdAsync(clinicaID);

                if (clinic == null)
                {
                    return _operationResult.ErrorResult($"Clinic not found with ID: {clinicaID}", new[] { "Clinic not found." });
                }

                var deleteResult = await _userManager.DeleteAsync(clinic);

                if (!deleteResult.Succeeded)
                {
                    return _operationResult.ErrorResult($"Failed to delete clinic:", deleteResult.Errors.Select(e => e.Description));
                }

                _common.AddInformationMessage("Clinic deleted successfully!");

                return _operationResult.SuccessResult("Clinic deleted successfully!");
            }
            catch (Exception ex)
            {
                _common.AddErrorMessage($"Error deleting clinic: {ex.Message}");
                return _operationResult.ErrorResult($"Failed to delete clinic:", new[] { ex.Message });
            }
        }

        public async Task<IEnumerable<ClinicRequest>> GetAllClinics()
        {
            var clinics = await _clinicRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClinicRequest>>(clinics);
        }

        public async Task<ClinicRequest> GetClinic(string clinicId)
        {
            var clinic = await _userManager.FindByIdAsync(clinicId);

            if (clinic == null)
            {
                return null;
            }

            return _mapper.Map<ClinicRequest>(clinic);
        }

        public async Task<OperationResult> UpdateClinic(string userId, ClinicRequest clinicRequest)
        {
            try
            {
                var existingClinic = await _userManager.FindByIdAsync(userId);

                if (existingClinic == null)
                {
                    return _operationResult.ErrorResult("Failed to update clinic:", new[] { "Clinic not found!" });
                }

                var clinicExists = await CheckIfClinicExists(clinicRequest.Email, userId);

                if (clinicExists != null && clinicExists.Id != userId)
                {
                    return _operationResult.ErrorResult("Failed to update clinic:", new[] { "This email is already associated with another clinic!" });
                }

                // Update other properties
                UpdateClinicProperties(existingClinic, clinicRequest);

                if (!string.IsNullOrEmpty(clinicRequest.Password))
                {
                    // Update password only if provided
                    var passwordHasher = new PasswordHasher<ApplicationUser>();
                    var hashedPassword = passwordHasher.HashPassword(existingClinic, clinicRequest.Password);
                    existingClinic.PasswordHash = hashedPassword;
                }

                var result = await _userManager.UpdateAsync(existingClinic);

                if (!result.Succeeded)
                {
                    return _operationResult.ErrorResult($"Failed to update clinic:", result.Errors.Select(e => e.Description));
                }

                _common.AddInformationMessage("Clinic updated successfully!");

                return _operationResult.SuccessResult("Clinic updated successfully!");
            }
            catch (FluentValidation.ValidationException validationException)
            {
                return _operationResult.ErrorResult("Failed to update patient: Validation error", validationException.Errors.Select(e => e.ErrorMessage));
            }
            catch (Exception ex)
            {
                _common.AddErrorMessage($"Error updating clinic: {ex.Message}");
                return _operationResult.ErrorResult($"Failed to update clinic:", new[] { ex.Message });
            }
        }

        private void UpdateClinicProperties(ApplicationUser existingPatient, ClinicRequest clinicRequest)
        {
            _mapper.Map(clinicRequest, existingPatient);

            if (existingPatient is Clinic patientToUpdate)
            {
                _mapper.Map(clinicRequest, patientToUpdate);
            }
        }
    }
}
