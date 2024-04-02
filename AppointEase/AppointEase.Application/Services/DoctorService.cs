using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Data.Contracts.Identity;
using AppointEase.Data.Contracts.Interfaces;
using AppointEase.Data.Contracts.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AppointEase.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IRepository<Doctor> _doctorRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IApplicationExtensions _common;
        private readonly IOperationResult _operationResult;

        public DoctorService(IRepository<Doctor> doctorRepository, UserManager<ApplicationUser> userManager, IMapper mapper, IApplicationExtensions common, IOperationResult operationResult)
        {
            _doctorRepository = doctorRepository;
            _userManager = userManager;
            _mapper = mapper;
            _common = common;
            _operationResult = operationResult;
        }

        public async Task<OperationResult> CreateDoctorAsync(DoctorRequest doctorRequest)
        {
            try
            {
                var doctorExists = await CheckIfDoctorExists(doctorRequest.Email, doctorRequest.PersonalNumber, null);

                if (doctorExists != null)
                {
                    return _operationResult.ErrorResult("Failed to create doctor:", new[] { "This doctor exists, please try again!" });
                }

                var user = _mapper.Map<Doctor>(doctorRequest);
                user.ClinicId = doctorRequest.ClinicId; // Set ClinicId

                var result = await _userManager.CreateAsync(user, doctorRequest.Password);

                if (!result.Succeeded)
                {
                    return _operationResult.ErrorResult($"Failed to create user:", result.Errors.Select(e => e.Description));
                }

                await _userManager.AddToRoleAsync(user, user.Role);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                _common.SendEmailConfirmation(token, user.Email);

                _common.AddInformationMessage("Doctor created successfully!");

                return _operationResult.SuccessResult("Doctor created successfully!");
            }
            catch (FluentValidation.ValidationException validationException)
            {
                return _operationResult.ErrorResult("Failed to create doctor: Validation error", validationException.Errors.Select(e => e.ErrorMessage));
            }
            catch (Exception ex)
            {
                _common.AddErrorMessage($"Error creating doctor: {ex.Message}");
                return _operationResult.ErrorResult($"Failed to create user:", new[] { ex.Message });
            }
        }
        private async Task<Doctor> CheckIfDoctorExists(string email, int personalNumber, string currentUserId)
        {
            var doctors = await GetAllDoctors();

            var doctorRequest = doctors.FirstOrDefault(p =>
                p != null && (p.Email == email || p.PersonalNumber == personalNumber) && p.Id != currentUserId);

            if (doctorRequest != null)
            {
                return _mapper.Map<Doctor>(doctorRequest);
            }

            return null;
        }

        public async Task<OperationResult> DeleteDoctor(string doctorId)
        {
            try
            {
                var doctor = await _userManager.FindByIdAsync(doctorId);

                if (doctor == null)
                {
                    return _operationResult.ErrorResult($"Doctor not found with ID: {doctorId}", new[] { "Doctor not found." });
                }

                var deleteResult = await _userManager.DeleteAsync(doctor);

                if (!deleteResult.Succeeded)
                {
                    return _operationResult.ErrorResult($"Failed to delete doctor:", deleteResult.Errors.Select(e => e.Description));
                }

                _common.AddInformationMessage("Doctor deleted successfully!");

                return _operationResult.SuccessResult("Doctor deleted successfully!");
            }
            catch (Exception ex)
            {
                _common.AddErrorMessage($"Error deleting doctor: {ex.Message}");
                return _operationResult.ErrorResult($"Failed to delete doctor:", new[] { ex.Message });
            }
        }

        public async Task<IEnumerable<DoctorRequest>> GetAllDoctors()
        {
            var doctors = await _doctorRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DoctorRequest>>(doctors);
        }

        public async Task<DoctorRequest> GetDoctor(string doctorId)
        {
            var doctor = await _userManager.FindByIdAsync(doctorId);

            if (doctor == null)
            {
                return null;
            }

            return _mapper.Map<DoctorRequest>(doctor);
        }

        public async Task<IEnumerable<DoctorRequest>> GetAllDoctorsByClinicId(string clinicId)
        {
            // Assuming there's a method in your repository or service layer to get doctors by clinic ID
            var doctors = await _doctorRepository.GetDoctorsByClinicId(clinicId);

            // Map the retrieved doctors to DoctorRequest DTOs
            return _mapper.Map<IEnumerable<DoctorRequest>>(doctors);
        }




        public async Task<OperationResult> UpdateDoctor(string userId, DoctorRequest doctorRequest)
        {
            try
            {
                var existingDoctor = await _userManager.FindByIdAsync(userId);

                if (existingDoctor == null)
                {
                    return _operationResult.ErrorResult("Failed to update doctor:", new[] { "Doctor not found!" });
                }

                var doctorExists = await CheckIfDoctorExists(doctorRequest.Email, doctorRequest.PersonalNumber, userId);

                if (doctorExists != null && doctorExists.UserName != userId)
                {
                    return _operationResult.ErrorResult("Failed to update doctor:", new[] { "This email or personal number is already associated with another doctor!" });
                }
                if (!string.IsNullOrEmpty(doctorRequest.Password))
                {
                    var passwordHasher = new PasswordHasher<ApplicationUser>();
                    var hashedPassword = passwordHasher.HashPassword(existingDoctor, doctorRequest.Password);
                    existingDoctor.PasswordHash = hashedPassword;
                }
                UpdateDoctorProperties(existingDoctor, doctorRequest);

                var result = await _userManager.UpdateAsync(existingDoctor);

                if (!result.Succeeded)
                {
                    return _operationResult.ErrorResult($"Failed to update doctor:", result.Errors.Select(e => e.Description));
                }

                _common.AddInformationMessage("Doctor updated successfully!");

                return _operationResult.SuccessResult("Doctor updated successfully!");
            }
            catch (FluentValidation.ValidationException validationException)
            {
                return _operationResult.ErrorResult("Failed to update doctor: Validation error", validationException.Errors.Select(e => e.ErrorMessage));
            }
            catch (Exception ex)
            {
                _common.AddErrorMessage($"Error updating doctor: {ex.Message}");
                return _operationResult.ErrorResult($"Failed to update doctor:", new[] { ex.Message });
            }
        }

        private void UpdateDoctorProperties(ApplicationUser existingDoctor, DoctorRequest doctorRequest)
        {
            _mapper.Map(doctorRequest, existingDoctor);

            if (existingDoctor is Doctor doctorToUpdate)
            {
                _mapper.Map(doctorRequest, doctorToUpdate);
            }
        }

        public async Task<IEnumerable<object>> FilterDoctors(Func<IQueryable<DoctorRequest>, IQueryable<object>> query)
        {
            
            var allDoctors = await _doctorRepository.GetAllAsync();
            var mappedDoctors = allDoctors.Select(d => _mapper.Map<DoctorRequest>(d));
            var filteredDoctors = query(mappedDoctors.AsQueryable());

            if (filteredDoctors != null && filteredDoctors.Any())
            {
                return filteredDoctors.ToList();
            }
            else
            {
                return new List<object>();
            }

        }
    }
}
