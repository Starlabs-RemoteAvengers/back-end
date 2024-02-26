using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Interfaces
{
    public interface IClinicService
    {
        Task<ClinicRequest> GetClinic(string clinicId);
        Task<IEnumerable<ClinicRequest>> GetAllClinics();
        Task<OperationResult> CreateClinicAsync(ClinicRequest clinicDto);
        Task<OperationResult> UpdateClinic(string clinicId, ClinicRequest clinicDto);
        Task<OperationResult> DeleteClinic(string clinicId);
    }
}
