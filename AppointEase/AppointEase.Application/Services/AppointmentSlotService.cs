using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Data.Contracts.Interfaces;
using AppointEase.Data.Contracts.Models;
using AppointEase.Data.Repositories;
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
                var overlappingSlot = await CheckIfAppointmentSlotExists(appointmentSlot.AppointmentSlotId, appointmentSlot.DoctorId, appointmentSlot.DayOfWeek, appointmentSlot.StartTime, appointmentSlot.EndTime);
                if (overlappingSlot != null)
                {
                    string errorMessage = $"This Appointment Slot already exists for the specified doctor and time: {overlappingSlot.StartTime} to {overlappingSlot.EndTime}, please choose another time!";
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

        private async Task<AppointmentSlot> CheckIfAppointmentSlotExists(string id, string doctorId, DayOfWeek dayOfWeek, TimeSpan newStartTime, TimeSpan newEndTime)
        {
            // Get all existing appointment slots for the specified doctor and day of the week
            var existingAppointmentSlots = await GetAllAppointmentSlotsForDoctorAndDay(doctorId, dayOfWeek);

            // Check if there is an overlapping time slot
            var overlappingSlot = existingAppointmentSlots.FirstOrDefault(slot =>
                slot.AppointmentSlotId != id && // Exclude the current one being edited
                IsTimeSlotOverlap(newStartTime, newEndTime, slot.StartTime, slot.EndTime));

            return overlappingSlot;
        }

        private async Task<IEnumerable<AppointmentSlot>> GetAllAppointmentSlotsForDoctorAndDay(string doctorId, DayOfWeek dayOfWeek)
        {
            // Get all existing appointment slots
            var allAppointmentSlots = await _appointmentSlotRepository.GetAllAsync();

            // Filter appointment slots for the specified doctor and day of the week
            var filteredSlots = allAppointmentSlots
                .Where(slot =>
                    slot.DoctorId == doctorId &&
                    slot.DayOfWeek == dayOfWeek &&
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
                // Check if there is an overlapping time slot for the updated appointment
                var overlappingSlot = await CheckIfAppointmentSlotExists(appointmentSlotRequest.AppointmentSlotId, appointmentSlotRequest.DoctorId, appointmentSlotRequest.DayOfWeek, appointmentSlotRequest.StartTime, appointmentSlotRequest.EndTime);

                if (overlappingSlot != null)
                {
                    string errorMessage = $"This Appointment Slot already exists for the specified doctor and time: {overlappingSlot.StartTime} to {overlappingSlot.EndTime}, please choose another time!";
                    return _operationResult.ErrorResult("Failed to update Appointment Slot:", new[] { errorMessage });
                }

                var existingAppointmentSlot = await _appointmentSlotRepository.GetByIdAsync(id);

                if (existingAppointmentSlot != null)
                {
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
