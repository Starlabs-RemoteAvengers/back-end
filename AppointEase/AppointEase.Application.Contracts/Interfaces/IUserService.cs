using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using Microsoft.AspNetCore.Identity;

namespace AppointEase.Application.Contracts.Interfaces
{
    public interface IUserService
    {
        Task<Object> LogInAsync(string username, string password);
        Task<OperationResult> UserForgotPassword(string email);
        Task<OperationResult> UserResetPassword(PasswordRequest passwordRequest);
        Task<OperationResult> UserChangePassword(PasswordRequest passwordRequest);

    }
}
