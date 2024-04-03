using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Data.Contracts.Identity;
using AppointEase.Data.Contracts.Interfaces;
using AppointEase.Data.Repositories;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AppointEase.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOperationResult _operationResult;
        private readonly IApplicationExtensions _applicationExtensions;
        private readonly IEmailServices emailServices;
        private readonly IValidator<PasswordRequest> _pValidator;
        private readonly IRepository<ApplicationUser> _usersRepository;
        private readonly IMapper _mapper;


        public UserService(UserManager<ApplicationUser> userManager, IOperationResult operationResult, IApplicationExtensions applicationExtensions, IEmailServices emailServices, IValidator<PasswordRequest> validator, IRepository<ApplicationUser> usersRepository, IMapper mapper)
        {
            _userManager = userManager;
            _operationResult = operationResult;
            _applicationExtensions = applicationExtensions;
            this.emailServices = emailServices;
            this._pValidator = validator;
            _usersRepository= usersRepository;
            _mapper = mapper;
        }

        public async Task<Object> LogInAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);

                if (isPasswordValid)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var userRole = userRoles.FirstOrDefault();
                    var userId = user.Id;

                    var token = await _applicationExtensions.GenerateJwtTokenAsync(userId,username, userRole,new Dictionary<string, string>()
                    {
                        {"EmailConfirmation", user.EmailConfirmed.ToString()},
                        {"Photodata",user.PhotoData ?? ""},
                        {"Photoformat",user.PhotoFormat ?? ""},
                        {"Name",user.Name ?? ""},
                        {"Surname",user.Surname ?? ""}
                    });

                    return token;
                }
            }
            return _operationResult.ErrorResult($"Failed to sign in:", new[] { "User not Found!" });
        }

        public async Task<OperationResult> UserChangePassword(PasswordRequest passwordRequest)
        {
            try
            {
                _pValidator.ValidateAndThrow(passwordRequest);

                var user = await _userManager.FindByIdAsync(passwordRequest.UserId);

                if (user == null)
                {
                    return _operationResult.ErrorResult("Failed:", new[] { $"User with ID {passwordRequest.UserId} not found." });
                }

                var isCorrectPassword = await _userManager.CheckPasswordAsync(user, passwordRequest.OldPassword);
                if (!isCorrectPassword)
                {
                    return _operationResult.ErrorResult("Failed:", new[] { $"The provided password is incorrect." });
                }
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, passwordRequest.OldPassword, passwordRequest.NewPassword);

                if (!changePasswordResult.Succeeded)
                {
                    return _operationResult.ErrorResult("Failed to change password:", changePasswordResult.Errors.Select(e => e.Description));
                }


                return _operationResult.SuccessResult("Password changed successfully.");
            }
            catch (Exception ex)
            {
                return _operationResult.ErrorResult($"Failed: ", new[] { ex.Message });
            }

        }

        public async Task<OperationResult> UserForgotPassword(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var token = await _applicationExtensions.GenerateJwtTokenAsync(new Dictionary<string, string>()
                    {
                         { "UserId", user.Id }
                    });

                    string url = $"http://localhost:3000/reset-password/{Uri.EscapeDataString(token)}";
                    _applicationExtensions.SendEmail(token, email, url);

                    return _operationResult.SuccessResult($"Email sended successfully!");
                }
                return _operationResult.ErrorResult($"Failed: ", new[] { "User does;t exist!" });
            }
            catch (Exception ex)
            {
                return _operationResult.ErrorResult($"Failed: ", new[] { ex.Message });
            }
        }

        public async Task<OperationResult> UserResetPassword(PasswordRequest passwordRequest)
        {
            try
            {
                _pValidator.ValidateAndThrow(passwordRequest);
                var user = await _userManager.FindByIdAsync(passwordRequest.UserId);
                if (user == null)
                {
                    // Handle user not found scenario
                    return _operationResult.ErrorResult("Failed:", new[] { $"User with ID {passwordRequest.UserId} not found." });
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, passwordRequest.NewPassword);

                if (result.Succeeded)
                {
                    // Password reset successful
                    return _operationResult.SuccessResult("Password reset successful.");
                }
                else
                {
                    // Password reset failed
                    return _operationResult.ErrorResult("Password reset failed.", result.Errors.Select(e => e.Description));
                }
            }
            catch (FluentValidation.ValidationException validationException)
            {
                return _operationResult.ErrorResult("Failed to create patient: Validation error", validationException.Errors.Select(e => e.ErrorMessage));
            }
            catch (Exception ex)
            {
                return _operationResult.ErrorResult($"Failed: ", new[] { ex.Message });
            }
        }
        public async Task<IEnumerable<ApplicationUserRequest>> GetUsers()
        {
            var users= await _usersRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ApplicationUserRequest>>(users);
        }

        public async Task<ApplicationUserRequest> GetUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (userId == null)
            {
                return null;
            }
            return _mapper.Map<ApplicationUserRequest>(user);
        }
    }
}
