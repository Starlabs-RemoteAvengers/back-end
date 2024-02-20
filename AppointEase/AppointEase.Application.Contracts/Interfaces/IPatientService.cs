using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.ModelsDto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Interfaces
{
    public interface IPatientService
    {
        Task <PatientRequest> GetPerson(int patientId);
        Task <IEnumerable<PatientRequest>> GetAllPersons();
        Task <OperationResult>CreatePersonAsync(PatientRequest personDto);
        Task <OperationResult> UpdatePerson(int personId, PatientRequest personDto);
        Task <OperationResult> DeletePerson(int personId);
    }
}
