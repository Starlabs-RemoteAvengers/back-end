using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Data.Contracts.Identity;
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


        public UserService(UserManager<ApplicationUser> userManager, IOperationResult operationResult, IApplicationExtensions applicationExtensions, IEmailServices emailServices, IValidator<PasswordRequest> validator)
        {
            _userManager = userManager;
            _operationResult = operationResult;
            _applicationExtensions = applicationExtensions;
            this.emailServices = emailServices;
            this._pValidator = validator;
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
                         { "EmailConfirmation", user.EmailConfirmed.ToString() }
                    });

                    return token;
                }
            }
            return _operationResult.ErrorResult($"Failed to sign in:", new[] { "User not Found!" });
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
    }
}
