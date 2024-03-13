using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Interfaces
{
    public interface IDoctorService
    {
        Task<DoctorRequest> GetDoctor(string doctorId);
        Task<IEnumerable<DoctorRequest>> GetAllDoctors();
        Task<IEnumerable<DoctorRequest>> GetAllDoctorsByClinicId(string clinicId);
        Task<OperationResult> CreateDoctorAsync(DoctorRequest personDto);
        Task<OperationResult> UpdateDoctor(string personId, DoctorRequest personDto);
        Task<OperationResult> DeleteDoctor(string personId);
        Task<IEnumerable<object>> FilterDoctors(Func<IQueryable<DoctorRequest>, IQueryable<object>> query);
    }
}
