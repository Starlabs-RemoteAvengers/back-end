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

namespace AppointEase.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepository<Admin> _adminRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IApplicationExtensions _common;
        private readonly IOperationResult _operationResult;

        public AdminService(IRepository<Admin> adminRepository, UserManager<ApplicationUser> userManager, IMapper mapper, IApplicationExtensions common, IOperationResult operationResult)
        {
            _adminRepository = adminRepository;
            _userManager = userManager;
            _mapper = mapper;
            _common = common;
            _operationResult = operationResult;
        }

        public async Task<OperationResult> CreateAdminAsync(AdminRequest adminRequest)
        {
            try
            {

                var adminExists = await CheckIfAdminExists(adminRequest.Email, adminRequest.PersonalNumber, null);

                if (adminExists != null)
                {
                    return _operationResult.ErrorResult("Failed to create admin:", new[] { "This admin exists, please try again!" });
                }

                var user = _mapper.Map<Admin>(adminRequest);

                var result = await _userManager.CreateAsync(user, adminRequest.Password);

                if (!result.Succeeded)
                {
                    return _operationResult.ErrorResult($"Failed to create user:", result.Errors.Select(e => e.Description));
                }

                await _userManager.AddToRoleAsync(user, user.Role);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                _common.SendEmailConfirmation(token, user.Email);

                _common.AddInformationMessage("Admin created successfully!");

                return _operationResult.SuccessResult("Admin created successfully!");
            }
            catch (FluentValidation.ValidationException validationException)
            {
                return _operationResult.ErrorResult("Failed to create admin: Validation error", validationException.Errors.Select(e => e.ErrorMessage));
            }
            catch (Exception ex)
            {
                _common.AddErrorMessage($"Error creating admin: {ex.Message}");
                return _operationResult.ErrorResult($"Failed to create user:", new[] { ex.Message });
            }
        }
        private async Task<Admin> CheckIfAdminExists(string email, int personalNumber, string currentUserId)
        {
            var admins = await GetAllAdmins();

            var adminRequest = admins.FirstOrDefault(p =>
                p != null && (p.Email == email || p.PersonalNumber == personalNumber) && p.Id != currentUserId);

            if (adminRequest != null)
            {
                return _mapper.Map<Admin>(adminRequest);
            }

            return null;
        }

        public async Task<OperationResult> DeleteAdmin(string adminId)
        {
            try
            {
                var admin = await _userManager.FindByIdAsync(adminId);

                if (admin == null)
                {
                    return _operationResult.ErrorResult($"Admin not found with ID: {adminId}", new[] { "Admin not found." });
                }

                var deleteResult = await _userManager.DeleteAsync(admin);

                if (!deleteResult.Succeeded)
                {
                    return _operationResult.ErrorResult($"Failed to delete admin:", deleteResult.Errors.Select(e => e.Description));
                }

                _common.AddInformationMessage("Admin deleted successfully!");

                return _operationResult.SuccessResult("Admin deleted successfully!");
            }
            catch (Exception ex)
            {
                _common.AddErrorMessage($"Error deleting admin: {ex.Message}");
                return _operationResult.ErrorResult($"Failed to delete admin:", new[] { ex.Message });
            }
        }
        public async Task<IEnumerable<AdminRequest>> GetAllAdmins()
        {
            var admins = await _adminRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AdminRequest>>(admins);
        }
        public async Task<AdminRequest> GetAdmin(string adminId)
        {
            var admin = await _userManager.FindByIdAsync(adminId);

            if (admin == null)
            {
                return null;
            }

            return _mapper.Map<AdminRequest>(admin);
        }
        public async Task<OperationResult> UpdateAdmin(string userId, AdminRequest adminRequest)
        {
            try
            {
                var existingAdmin = await _userManager.FindByIdAsync(userId);

                if (existingAdmin == null)
                {
                    return _operationResult.ErrorResult("Failed to update admin:", new[] { "Admin not found!" });
                }

                var adminExists = await CheckIfAdminExists(adminRequest.Email, adminRequest.PersonalNumber, userId);

                if (adminExists != null && adminExists.UserName != userId)
                {
                    return _operationResult.ErrorResult("Failed to update admin:", new[] { "This email or personal number is already associated with another admin!" });
                }

                if (!string.IsNullOrEmpty(adminRequest.Password))
                {
                    var passwordHasher = new PasswordHasher<ApplicationUser>();
                    var hashedPassword = passwordHasher.HashPassword(existingAdmin, adminRequest.Password);
                    existingAdmin.PasswordHash = hashedPassword;
                }

                UpdateAdminProperties(existingAdmin, adminRequest);

                var result = await _userManager.UpdateAsync(existingAdmin);

                if (!result.Succeeded)
                {
                    return _operationResult.ErrorResult($"Failed to update admin:", result.Errors.Select(e => e.Description));
                }

                _common.AddInformationMessage("Admin updated successfully!");

                return _operationResult.SuccessResult("Admin updated successfully!");
            }
            catch (FluentValidation.ValidationException validationException)
            {
                return _operationResult.ErrorResult("Failed to update admin: Validation error", validationException.Errors.Select(e => e.ErrorMessage));
            }
            catch (Exception ex)
            {
                _common.AddErrorMessage($"Error updating admin: {ex.Message}");
                return _operationResult.ErrorResult($"Failed to update admin:", new[] { ex.Message });
            }
        }
        private void UpdateAdminProperties(ApplicationUser existingAdmin, AdminRequest adminRequest)
        {
            // Use AutoMapper to map properties from PatientRequest to ApplicationUser
            _mapper.Map(adminRequest, existingAdmin);

            if (existingAdmin is Admin adminToUpdate)
            {
                // Additional mapping for properties specific to the Patient class
                _mapper.Map(adminRequest, adminToUpdate);
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
