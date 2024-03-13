using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Interfaces
{
    public interface IAppointmentSlotService
    {
        Task<AppointmentSlotRequest> GetAppointmentById(string id);
        Task<IEnumerable<AppointmentSlotRequest>> GetAllAppointmentSlots();
        Task<OperationResult> CreateAppointmentSlot(AppointmentSlotRequest appointmentSlot);
        Task<OperationResult> CreateAppointmentSlotByWeeks(AppointmentSlotRequest appointmentSlot, int numberOfWeeks);
        Task<OperationResult> UpdateAppointmentSlot(string id, AppointmentSlotRequest appointmentSlot);
        Task<OperationResult> DeleteAsync(string id);
    }
}
