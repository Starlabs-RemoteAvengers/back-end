using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Data.Contracts.Interfaces;
using AppointEase.Data.Contracts.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Services
{
    public class AppointmentSlotService : IAppointmentSlotService
    {
        private readonly IRepository<AppointmentSlot> _appointmentSlotRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationExtensions _common;
        private readonly IOperationResult _operationResult;
        private readonly ILogger<AppointmentSlotService> _logger;
        public AppointmentSlotService(IMapper mapper, IApplicationExtensions common, IOperationResult operationResult, IRepository<AppointmentSlot> appointmentSlotRepository, ILogger<AppointmentSlotService> logger)
        {
            _mapper = mapper;
            _common = common;
            _operationResult = operationResult;
            _appointmentSlotRepository = appointmentSlotRepository;
            _logger = logger;
        }

        public async Task<OperationResult> CreateAppointmentSlot(AppointmentSlotRequest appointmentSlot)
        {
            try
            {
                var overlappingSlot = await CheckIfAppointmentSlotExists(appointmentSlot.AppointmentSlotId, appointmentSlot.DoctorId, appointmentSlot.Date, appointmentSlot.StartTime, appointmentSlot.EndTime);
                if (overlappingSlot != null)
                {
                    string errorMessage = $"This Appointment Slot already exists for the specified doctor date and time:{overlappingSlot.Date} {overlappingSlot.StartTime} to {overlappingSlot.EndTime}, please choose another time!";
                    return _operationResult.ErrorResult("Failed to create Appointment Slot:", new[] { errorMessage });
                }

                var appointment = _mapper.Map<AppointmentSlot>(appointmentSlot);
                var result = await _appointmentSlotRepository.AddAsync(appointment);

                if (!result.Succeeded)
                {
                    return _operationResult.ErrorResult($"Failed to create user:", result.Errors);
                }

                _common.AddInformationMessage("Appointment Slot Created Successfully!");
                return _operationResult.SuccessResult("Appointment Slot Created Successfully!");
            }
            catch (Exception exception)
            {
                _common.AddErrorMessage($"Error creating AppointmentSlot: {exception.Message}");
                return _operationResult.ErrorResult($"Failed to create Appointment Slot", new[] { exception.Message });
            }
        }

        public async Task<OperationResult> CreateAppointmentSlotByWeeks(AppointmentSlotRequest appointmentSlot, int numberOfWeeks)
        {
            try
            {
                List<OperationResult> results = new List<OperationResult>();

                for (int week = 0; week < numberOfWeeks; week++)
                {
                    DateOnly newDate = appointmentSlot.Date.AddDays(7 * week);

                    var overlappingSlot = await CheckIfAppointmentSlotExists(appointmentSlot.AppointmentSlotId, appointmentSlot.DoctorId, newDate, appointmentSlot.StartTime, appointmentSlot.EndTime);

                    if (overlappingSlot != null)
                    {
                        string errorMessage = $"This Appointment Slot already exists for the specified doctor and time:{overlappingSlot.Date} - {overlappingSlot.StartTime} to {overlappingSlot.EndTime}, please choose another time!";
                        results.Add(_operationResult.ErrorResult($"Failed to create Appointment Slot for {newDate.ToShortDateString()}:", new[] { errorMessage }));
                    }
                    else
                    {
                        var newAppointmentSlot = _mapper.Map<AppointmentSlot>(appointmentSlot);
                        newAppointmentSlot.Date = newDate;

                        var result = await _appointmentSlotRepository.AddAsync(newAppointmentSlot);

                        if (!result.Succeeded)
                        {
                            results.Add(_operationResult.ErrorResult($"Failed to create Appointment Slot for {newDate.ToShortDateString()}: {result.Errors}"));
                        }
                        else
                        {
                            _common.AddInformationMessage($"Appointment Slot Created Successfully for {newDate.ToShortDateString()}!");
                            results.Add(_operationResult.SuccessResult($"Appointment Slot Created Successfully for {newDate.ToShortDateString()}!"));
                        }
                    }
                }
                return CombineResults(results);
            }
            catch (Exception exception)
            {
                _common.AddErrorMessage($"Error creating AppointmentSlots: {exception.Message}");
                return _operationResult.ErrorResult($"Failed to create Appointment Slots", new[] { exception.Message });
            }
        }


        private OperationResult CombineResults(List<OperationResult> results)
        {
            if (results == null || results.Count == 0)
            {
                return _operationResult.ErrorResult("No results provided.");
            }

            bool allSuccess = results.All(result => result.Succeeded);
            string[] errorMessages = results
                .Where(result => !result.Succeeded)
                .SelectMany(result => result.Errors)
                .ToArray();

            if (allSuccess)
            {
                return _operationResult.SuccessResult("All Appointment Slots Created Successfully!");
            }
            else
            {
                string errorMessage = string.Join(Environment.NewLine, errorMessages);
                return _operationResult.ErrorResult("Failed to create some Appointment Slots:", errorMessage);
            }
        }

        private async Task<AppointmentSlot> CheckIfAppointmentSlotExists(string id, string doctorId, DateOnly newDate, TimeSpan newStartTime, TimeSpan newEndTime)
        {
            // Get all existing appointment slots for the specified doctor and day
            var existingAppointmentSlots = await GetAllAppointmentSlotsForDoctorAndDay(doctorId, newDate);

            // Check if there is an overlapping time slot excluding the current one being updated
            var overlappingSlot = existingAppointmentSlots.FirstOrDefault(slot =>
                slot.AppointmentSlotId != id && // Exclude the current one being edited
                IsTimeSlotOverlap(newStartTime, newEndTime, slot.StartTime, slot.EndTime));

            return overlappingSlot;
        }

        private async Task<IEnumerable<AppointmentSlot>> GetAllAppointmentSlotsForDoctorAndDay(string doctorId, DateOnly date)
        {
            // Get all existing appointment slots
            var allAppointmentSlots = await _appointmentSlotRepository.GetAllAsync();

            // Filter appointment slots for the specified doctor and day
            var filteredSlots = allAppointmentSlots
                .Where(slot =>
                    slot.DoctorId == doctorId &&
                    slot.Date == date &&
                    IsTimeSlotOverlap(slot.StartTime, slot.EndTime, slot.StartTime, slot.EndTime)) // Assuming you want appointments between 11:00 AM and 12:00 PM
                .ToList();

            return filteredSlots;
        }


        private bool IsTimeSlotOverlap(TimeSpan newStartTime, TimeSpan newEndTime, TimeSpan existingStartTime, TimeSpan existingEndTime)
        {
            // Check if the new time slot overlaps with the existing one
            return newStartTime < existingEndTime && existingStartTime < newEndTime;
        }


        public async Task<IEnumerable<AppointmentSlotRequest>> GetAllAppointmentSlots()
        {
            try
            {
                var appointmentSlots = await _appointmentSlotRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<AppointmentSlotRequest>>(appointmentSlots);
            }
            catch (Exception exception)
            {
                _common.AddErrorMessage($"Error retrieving all AppointmentSlots: {exception.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<AppointmentSlotRequest>> GetAppointmentSlotsByDoctorId(string doctorId)
        {
            try {
                var appointmentSlotsByDoctorId = await _appointmentSlotRepository.GetAppointmentSlotsByDoctorId(doctorId);
                return _mapper.Map<IEnumerable<AppointmentSlotRequest>>(appointmentSlotsByDoctorId);
            } 
            catch(Exception exception) 
            {
                _common.AddErrorMessage($"Error retrieving all AppointmentSlots: {exception.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<AppointmentSlotRequest>> GetMyDoctorsAppointmentSlots(string clinicId)
        {
            try
            {
                var appointmentSlot = await _appointmentSlotRepository.GetMyDoctorsAppointmentSlots(clinicId);
                return _mapper.Map<IEnumerable<AppointmentSlotRequest>>(appointmentSlot);
            }
            catch(Exception exception)
            {
                _common.AddErrorMessage($"Error retrieving data: {exception.Message}");
                throw;
            }
        }

        public async Task<AppointmentSlotRequest> GetAppointmentById(string id)
        {
            try
            {
                var appointmentSlot = await _appointmentSlotRepository.GetByIdAsync(id);
                return _mapper.Map<AppointmentSlotRequest>(appointmentSlot);
            }
            catch (Exception exception)
            {
                _common.AddErrorMessage($"Error retrieving AppointmentSlot by ID: {exception.Message}");
                throw;
            }
        }

        public async Task<OperationResult> UpdateAppointmentSlot(string id, AppointmentSlotRequest appointmentSlotRequest)
        {
            try
            {
                var existingAppointmentSlot = await _appointmentSlotRepository.GetByIdAsync(id);

                if (existingAppointmentSlot != null)
                {
                    // Check if the relevant properties are being modified
                    bool isTimeModified = existingAppointmentSlot.Date != appointmentSlotRequest.Date ||
                                         existingAppointmentSlot.StartTime != appointmentSlotRequest.StartTime ||
                                         existingAppointmentSlot.EndTime != appointmentSlotRequest.EndTime;

                    // If time-related properties are modified, check for overlapping slots
                    if (isTimeModified)
                    {
                        var overlappingSlot = await CheckIfAppointmentSlotExists(id, appointmentSlotRequest.DoctorId, appointmentSlotRequest.Date, appointmentSlotRequest.StartTime, appointmentSlotRequest.EndTime);

                        if (overlappingSlot != null)
                        {
                            string errorMessage = $"This Appointment Slot already exists for the specified doctor and time: {overlappingSlot.StartTime} to {overlappingSlot.EndTime}, please choose another time!";
                            return _operationResult.ErrorResult("Failed to update Appointment Slot:", new[] { errorMessage });
                        }
                    }

                    // Update other attributes without checking for overlap
                    _mapper.Map(appointmentSlotRequest, existingAppointmentSlot);
                    var result = await _appointmentSlotRepository.UpdateAsync(existingAppointmentSlot);

                    if (!result.Succeeded)
                    {
                        return _operationResult.ErrorResult($"Failed to update Appointment Slot:", result.Errors);
                    }

                    _common.AddInformationMessage("Appointment Slot Updated Successfully!");
                    return _operationResult.SuccessResult("Appointment Slot Updated Successfully!");
                }
                else
                {
                    return _operationResult.ErrorResult("Appointment Slot not found", new string[] { });
                }
            }
            catch (Exception exception)
            {
                _common.AddErrorMessage($"Error updating AppointmentSlot: {exception.Message}");
                return _operationResult.ErrorResult("Failed to update Appointment Slot", new[] { exception.Message });
            }
        }

        public async Task<OperationResult> DeleteAsync(string id)
        {
            try
            {
                var appointmentSlotToDelete = await _appointmentSlotRepository.GetByIdAsync(id);
                _logger.LogInformation($"Appointment Slot with ID {id} {(appointmentSlotToDelete == null ? "not found" : "found")}");
                if (appointmentSlotToDelete == null)
                {
                    return _operationResult.ErrorResult($"Appointment Slot not found with ID: {id}", new[] { "Appointment Slot not found." });
                }

                // Attempt to delete the appointment slot
                var deleteResult = await _appointmentSlotRepository.DeleteAsync(id);

                if (!deleteResult.Succeeded)
                {
                    // The entity wasn't found or couldn't be deleted
                    return _operationResult.ErrorResult("Failed to delete Appointment Slot", deleteResult.Errors);
                }

                _common.AddInformationMessage("Appointment Slot deleted successfully!");
                return _operationResult.SuccessResult("Appointment Slot deleted successfully!");
            }
            catch (Exception exception)
            {
                // Handle generic exceptions
                _common.AddErrorMessage($"Error deleting AppointmentSlot: {exception.Message}");
                return _operationResult.ErrorResult("Failed to delete Appointment Slot", new[] { exception.Message });
            }
        }
    }
}
