using AppointEase.Application.Contracts.Identity;
using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.DbModels;
using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.ModelsDto;
using AppointEase.AspNetCore.Validator;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IRepository<TblPacient> _uRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ICommon _common;
        private readonly IValidator<PatientRequest> _personValidator;

        public PatientService(IRepository<TblPacient> uRepository, UserManager<ApplicationUser> userManager, IMapper mapper, ICommon common, IValidator<PatientRequest> validator)
        {
            _uRepository = uRepository;
            _userManager = userManager;
            _mapper = mapper;
            _common = common;
            _personValidator = validator;
        }

        public async Task<OperationResult> CreatePersonAsync(PatientRequest patientRequest)
        {
            try
            {
                var patient = _mapper.Map<TblPacient>(patientRequest);
                var patients = await GetAllPersons();

                _personValidator.ValidateAndThrow(patientRequest);

                var patientExcist = from pExcist in patients
                                    where pExcist != null && (pExcist.Email == patientRequest.Email || pExcist.PersonalNumber == patientRequest.PersonalNumber)
                                    select pExcist;

                if (patientExcist?.Count() > 0)
                    return  OperationResult.SuccessResult("Patient created successfully!");

                var user = new ApplicationUser
                {
                    UserName = patientRequest.Email,
                    Email = patientRequest.Email,
                    Name = patientRequest.Name,
                    Surname = patientRequest.Surname,
                    Role = ICommon.Role,
                    PersonalNumber = patientRequest.PersonalNumber,
                };

                var result = await _userManager.CreateAsync(user, patientRequest.Password);

                if (!result.Succeeded)
                    return OperationResult.ErrorResult($"Failed to create user:", result.Errors.Select(e => e.Description));

                await _userManager.AddToRoleAsync(user, user.Role);
                await _uRepository.AddAsync(patient);

                _common.LogInformation("Patient created successfully!");

                return OperationResult.SuccessResult("Patient created successfully!");

            }
            catch (FluentValidation.ValidationException validationException)
            {
                return OperationResult.ErrorResult("Failed to create user: Validation error", validationException.Errors.Select(e => e.ErrorMessage));
            }
            catch (Exception ex)
            {
                _common.LogError($"Error creating patient: {ex.Message}");
                return OperationResult.ErrorResult($"Failed to create user:" , new[] { ex.Message });
            }
        }

        public async Task<OperationResult> DeletePerson(int personId)
        {
            var person = await GetPerson(personId);
            var user = await _userManager.FindByEmailAsync(person.Email);

            if (user == null)
                return OperationResult.ErrorResult($"Not Found:", new[] {"User doesn't excist!"});

            // Fshij përdoruesin nga baza e të dhënave
            var result = await _userManager.DeleteAsync(user);
            await _uRepository.DeleteAsync(personId);

            return OperationResult.SuccessResult("Patient Deleted successfully!");
        }

        public async Task<IEnumerable<PatientRequest>> GetAllPersons()
        {
            var persons = await _uRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PatientRequest>>(persons);
        }

        public async Task<PatientRequest> GetPerson(int patientId)
        {
            var persons = _uRepository.GetByIdAsync(patientId);
            return _mapper.Map<PatientRequest>(persons);
        }
        public async Task<OperationResult> UpdatePerson(int personId, PatientRequest patientRequest)
        {
            try
            {
                var existingPerson = await _uRepository.GetByIdAsync(personId);
                if (existingPerson == null)
                    return OperationResult.ErrorResult("Person not found", new[] {"Error1"});

                // Validate patientRequest using FluentValidation
                _personValidator.ValidateAndThrow(patientRequest);

                var user = new ApplicationUser
                {
                    UserName = patientRequest.Email,
                    Email = patientRequest.Email,
                    Name = patientRequest.Name,
                    Surname = patientRequest.Surname,
                    Role = ICommon.Role,
                    PersonalNumber = patientRequest.PersonalNumber,
                };
                await _userManager.UpdateAsync(user);

                _mapper.Map(patientRequest, existingPerson);
                await _uRepository.UpdateAsync(existingPerson);

                return OperationResult.SuccessResult();
            }
            catch (FluentValidation.ValidationException validationException)
            {
                return OperationResult.ErrorResult("Failed to update user: Validation error", validationException.Errors.Select(e => e.ErrorMessage));
            }
            catch (Exception ex)
            {
                _common.LogError($"Error updating patient: {ex.Message}");
                return OperationResult.ErrorResult($"Failed to create user:", new[] { ex.Message });
            }
           
        }
    }
}
