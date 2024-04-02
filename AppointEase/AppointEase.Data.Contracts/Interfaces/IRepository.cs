using AppointEase.Application.Contracts.Models;
using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Data.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Data.Contracts.Interfaces
{
    public interface IRepository <T>
    {
        Task<string> GetIdByEmailAndPasswordAsync(string email, string password);
        Task<T> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<OperationResult> AddAsync(T entity);
        Task<OperationResult> UpdateAsync(T entity);
        Task<OperationResult> DeleteAsync(string id);
        Task<IEnumerable<T>> GetDoctorsByClinicId(string clinicId);
        Task<IEnumerable<AppointmentSlot>>GetAppointmentSlotsByDoctorId(string doctorId);
        Task<IEnumerable<AppointmentSlot>> GetMyDoctorsAppointmentSlots(string clinicId);
        Task<string> GetPhoneNumberByIdAsync(string userId);
    }
}
