using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Data.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Application.Contracts.ModelsRespond;
using AppointEase.Data.Contracts.Identity;
using AppointEase.Data.Contracts.Models;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System.Data;


namespace AppointEase.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IRepository<TblPacient> _uRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IApplicationExtensions _common;
        private readonly IOperationResult _operationResult;

        public PatientService(IRepository<TblPacient> uRepository, UserManager<ApplicationUser> userManager, IMapper mapper, IApplicationExtensions common,IOperationResult operation)
        {
            _uRepository = uRepository;
            _userManager = userManager;
            _mapper = mapper;
            _common = common;
            _operationResult = operation;
        }

        public async Task<OperationResult> CreatePatitentAsync(PatientRequest patientRequest)
        {
            try
            {

                var patient = _mapper.Map<TblPacient>(patientRequest);
                var patients = await GetAllPatitents();


                var patientExcist = from pExcist in patients
                                    where pExcist != null && (pExcist.Email == patientRequest.Email || pExcist.PersonalNumber == patientRequest.PersonalNumber)
                                    select pExcist;

                if (patientExcist?.Count() > 0)
                      return _operationResult.ErrorResult("Failed to create patient:", new[] { "This patient exists, please try again!" });

                var user = _mapper.Map<ApplicationUser>(patientRequest);

                var result = await _userManager.CreateAsync(user, patientRequest.Password);

                if (!result.Succeeded)
                    return _operationResult.ErrorResult($"Failed to create user:", result.Errors.Select(e => e.Description));

                await _userManager.AddToRoleAsync(user, user.Role);
                await _uRepository.AddAsync(patient);

                _common.AddInformationMessage("Patient created successfully!");

                return _operationResult.SuccessResult("Patient created successfully!");

            }
            catch (FluentValidation.ValidationException validationException)
            {
                return  _operationResult.ErrorResult("Failed to create patient: Validation error", validationException.Errors.Select(e => e.ErrorMessage));
            }
            catch (Exception ex)
            {
                _common.AddErrorMessage($"Error creating patient: {ex.Message}");
                return _operationResult.ErrorResult($"Failed to create user:" , new[] { ex.Message });
            }
        }

        public async Task<OperationResult> DeletePatitent(int personId)
        {
            var person = await GetPatitent(personId);
            var user = await _userManager.FindByEmailAsync(person.Email);

            if (user == null)
                return _operationResult.ErrorResult($"Not Found:", new[] { "User doesn't excist!" });

            // Fshij përdoruesin nga baza e të dhënave
            var result = await _userManager.DeleteAsync(user);
            await _uRepository.DeleteAsync(personId);

            return _operationResult.SuccessResult("Patient Deleted successfully!");
        }

        public async Task<IEnumerable<PatientResponse>> GetAllPatitents()
        {
            var persons = await _uRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PatientResponse>>(persons);
        }

        public async Task<PatientResponse> GetPatitent(int patientId)
        {
            var persons = _uRepository.GetByIdAsync(patientId);
            return _mapper.Map<PatientResponse>(persons);
        }

        public async Task<OperationResult> UpdatePatitent(int personId, PatientRequest patientRequest)
        {
            try
            {
                var existingPerson = await _uRepository.GetByIdAsync(personId);
                if (existingPerson == null)
                    return _operationResult.ErrorResult("Person not found", new[] { "Error1" });

                // Validate patientRequest using FluentValidation
                //_personValidator.ValidateAndThrow(patientRequest);
                var user = _mapper.Map<ApplicationUser>(patientRequest);
               
                await _userManager.UpdateAsync(user);

                _mapper.Map(patientRequest, existingPerson);
                await _uRepository.UpdateAsync(existingPerson);

                return _operationResult.SuccessResult();
            }
            catch (FluentValidation.ValidationException validationException)
            {
                return _operationResult.ErrorResult("Failed to update user: Validation error", validationException.Errors.Select(e => e.ErrorMessage));
            }
            catch (Exception ex)
            {
                _common.AddErrorMessage($"Error updating patient: {ex.Message}");
                return _operationResult.ErrorResult($"Failed to create user:", new[] { ex.Message });
            }
        }
       
    }
}
