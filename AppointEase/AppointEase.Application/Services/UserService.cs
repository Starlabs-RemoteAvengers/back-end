using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Data.Contracts.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOperationResult _operationResult;
        public UserService(UserManager<ApplicationUser> userManager, IOperationResult operationResult)
        {
            _userManager = userManager;
            _operationResult = operationResult;
        }

        public async Task<OperationResult> LogInAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);

                if (isPasswordValid)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var userRole = userRoles.FirstOrDefault();

                    return _operationResult.SuccessResult("User created successfully!");
                }
            }
            return _operationResult.ErrorResult($"Failed to create user:", new[] { "test" });
        }
    }
}
