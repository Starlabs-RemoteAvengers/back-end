using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Application.Contracts.ModelsRespond;

namespace AppointEase.Application.Contracts.Interfaces
{
    public interface IPatientService
    {
        Task <PatientResponse> GetPatitent(int patientId);
        Task <IEnumerable<PatientResponse>> GetAllPatitents();
        Task <OperationResult>CreatePatitentAsync(PatientRequest personDto);
        Task <OperationResult> UpdatePatitent(string personId, PatientRequest personDto);
        Task <OperationResult> DeletePatitent(int personId);
    }
}
