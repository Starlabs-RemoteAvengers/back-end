using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Interfaces
{
    public interface IAdminService
        {
            Task<AdminRequest> GetAdmin(string adminId);
            Task<IEnumerable<AdminRequest>> GetAllAdmins();
            Task<OperationResult> CreateAdminAsync(AdminRequest personDto);
            Task<OperationResult> UpdateAdmin(string personId, AdminRequest personDto);
            Task<OperationResult> DeleteAdmin(string personId);
            Task<OperationResult> ConfirmEmail(string token, string email);
        }
}
