using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Data.Contracts.Interfaces;
using AppointEase.Data.Contracts.Models;
using AppointEase.Data.Repositories;
using AppointEase.Http.Contracts.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Services
{
    public enum AppointmentStatus
    {
        Pending,
        Accepted,
        Declined,
        Canceled
    }
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationExtensions _common;
        private readonly IOperationResult _operationResult;
        private readonly ILogger<AppointmentSlotService> _logger;
        private readonly IRepository<BookAppointment> _bookappointmentRepository;
        private readonly IRepository<AppointmentSlot> _appointmentSlotRepository;
        private readonly ITwilioService _twilioService;
        private readonly IRepository<Patient> _patientRepository;
        private readonly IRepository<Doctor> _doctorRepository;
        public AppointmentService(IRepository<Appointment> appointmentRepository, IMapper mapper, IApplicationExtensions common, IOperationResult operationResult, ILogger<AppointmentSlotService> logger, IRepository<BookAppointment> bookappointmentRepository, IRepository<AppointmentSlot> appointmentSlotRepository, ITwilioService twilioService, IRepository<Patient> patientRepository, IRepository<Doctor> doctorRepository)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
            _common = common;
            _operationResult = operationResult;
            _logger = logger;
            _bookappointmentRepository = bookappointmentRepository;
            _appointmentSlotRepository = appointmentSlotRepository;
            _twilioService = twilioService;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<OperationResult> CreateAppointment(AppointmentRequest request)
        {
            try
            {
                // Check if the appointment exists
                var appointmentExists = await CheckIfAppointmentExists(request.BookAppointmentId);
                if (appointmentExists != null)
                {
                    return _operationResult.ErrorResult("Failed to create Appointment:", new[] { "This appointment exists, please try again!" });
                }
                // Map the AppointmentRequest to Appointment
                var appointment = _mapper.Map<Appointment>(request);

                var result = await _appointmentRepository.AddAsync(appointment);

                if (!result.Succeeded)
                {
                    return _operationResult.ErrorResult($"Failed to create user:", result.Errors);
                }

                if (request.Status == AppointmentStatus.Accepted.ToString() || request.Status == AppointmentStatus.Declined.ToString())
                {
                    // Update the status of the corresponding book appointment
                    var bookAppointment = await _bookappointmentRepository.GetByIdAsync(request.BookAppointmentId);

                    if (bookAppointment != null)
                    {
                        bookAppointment.BookAppointmentStatus = request.Status;
                        await _bookappointmentRepository.UpdateAsync(bookAppointment);

                        if (request.Status == AppointmentStatus.Declined.ToString())
                        {
                            // Update the status of the corresponding appointment slot if declined
                            var appointmentSlot = await _appointmentSlotRepository.GetByIdAsync(bookAppointment.AppointmentSlotId);
                            if (appointmentSlot != null)
                            {
                                appointmentSlot.IsBooked = false;
                                await _appointmentSlotRepository.UpdateAsync(appointmentSlot);
                            }
                        }
                    }
                }

                _common.AddInformationMessage("Appointment Slot Created Successfully!");
                return _operationResult.SuccessResult("Appointment Slot Created Successfully!");
            }
            catch (Exception exception)
            {
                _common.AddErrorMessage($"Error creating Appointment: {exception.Message}");
                return _operationResult.ErrorResult($"Failed to create Appointment:", new[] { exception.Message });
            }
        }

        private async Task<Appointment> CheckIfAppointmentExists(string bookAppointmentId)
        {

            var appointments = await GetAllAppointments();
            var appointmentsRequest = appointments.FirstOrDefault(a => a.BookAppointmentId == bookAppointmentId);
            if (appointmentsRequest != null)
            {
                return _mapper.Map<Appointment>(appointmentsRequest);
            }

            return null;
        }
        public async Task<OperationResult> DeleteAppointment(string id)
        {
            try
            {
                var getAppointment = await _appointmentRepository.GetByIdAsync(id);
                if (getAppointment == null)
                {
                    return _operationResult.ErrorResult($"Appointment not found with ID: {id}", new[] { "Appointment not found." });
                }

                var deleteAppointment = await _appointmentRepository.DeleteAsync(id);
                if (!deleteAppointment.Succeeded)
                {
                    // The entity wasn't found or couldn't be deleted
                    return _operationResult.ErrorResult("Failed to delete Appointment Slot", deleteAppointment.Errors);
                }

                _common.AddInformationMessage("Appointment deleted successfully!");

                return _operationResult.SuccessResult("Appointment deleted successfully!");
            }
            catch(Exception exception)
            {
                _common.AddErrorMessage($"Error deleting AppointmentSlot: {exception.Message}");
                return _operationResult.ErrorResult("Failed to delete Appointment Slot", new[] { exception.Message });
            }
        }

        public async Task<IEnumerable<AppointmentRequest>> GetAllAppointments()
        {
            try
            {
                var appointments = await _appointmentRepository.GetAllAsync();
                var appointmentRequests = new List<AppointmentRequest>();

                foreach (var appointment in appointments)
                {
                    var appointmentRequest = _mapper.Map<AppointmentRequest>(appointment);

                    // Fetch the corresponding BookAppointment object using the bookAppointmentId
                    var bookAppointment = await FetchBookAppointmentById(appointment.BookAppointmentId);

                    // Create a new BookAppointmentRequest object and set its properties
                    var bookAppointmentRequest = _mapper.Map<BookAppointmentRequest>(bookAppointment);

                    // Fetch and include the entire AppointmentSlot object
                    var appointmentSlot = await _appointmentSlotRepository.GetByIdAsync(bookAppointment.AppointmentSlotId);
                    var appointmentSlotRequest = _mapper.Map<AppointmentSlotRequest>(appointmentSlot);

                    // Assign the BookAppointmentRequest object to the AppointmentRequest's BookAppointment property
                    appointmentRequest.SetBookAppointment(bookAppointmentRequest);

                    // Assign the AppointmentSlotRequest object to the AppointmentRequest's AppointmentSlot property
                    appointmentRequest.SetAppointmentSlot(appointmentSlotRequest);

                    appointmentRequests.Add(appointmentRequest);
                }
                return appointmentRequests;
            }
            catch (Exception exception)
            {
                _common.AddErrorMessage($"Error retrieving all Appointments: {exception.Message}");
                throw;
            }
        }


        private async Task<BookAppointment> FetchBookAppointmentById(string bookAppointmentId)
        {
            try
            {
                return await _bookappointmentRepository.GetByIdAsync(bookAppointmentId);
            }
            catch (Exception exception)
            {
                _common.AddErrorMessage($"Error retrieving BookAppointment by ID: {exception.Message}");
                throw;
            }
        }


        public async Task<AppointmentRequest> GetAppointment(string id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            return _mapper.Map<AppointmentRequest>(appointment);
        }
        public async Task<OperationResult> AcceptAppointment(string id)
        {
            try
            {
                var appointments = await GetAllAppointments();
                var existingAppointment = appointments.FirstOrDefault(a => a.BookAppointmentId == id);
                if (existingAppointment != null)
                {
                    return _operationResult.ErrorResult($"Failed to accept Appointment Request: Appointment Request with ID {id} already exists.", new[] { "Appointment already exists." });
                }

                var bookAppointment = await _bookappointmentRepository.GetByIdAsync(id);
                if (bookAppointment == null)
                {
                    return _operationResult.ErrorResult($"Failed to accept Book Appointment: Book Appointment with ID {id} not found.", new[] { "Book Appointment not found." });
                }

                var appointment = new Appointment
                {
                    BookAppointmentId = id,
                    Status = AppointmentStatus.Accepted.ToString()
                };

                var result = await _appointmentRepository.AddAsync(appointment);

                if (!result.Succeeded)
                {
                    return _operationResult.ErrorResult($"Failed to accept Appointment:", result.Errors);
                }

                bookAppointment.BookAppointmentStatus = AppointmentStatus.Accepted.ToString();
                await _bookappointmentRepository.UpdateAsync(bookAppointment);

                var appointmentSlot = await _appointmentSlotRepository.GetByIdAsync(bookAppointment.AppointmentSlotId);
                var doctor = await _doctorRepository.GetByIdAsync(appointmentSlot.DoctorId);
                
                var patientId = bookAppointment.PatientId;
                
                // Fetch the phone number associated with the patientId from your repository or service
                var patientPhoneNumber = await _patientRepository.GetPhoneNumberByIdAsync(patientId);
                // Check if the phone number is not null or empty before sending the message
                if (!string.IsNullOrEmpty(patientPhoneNumber))
                {
                    _twilioService.SendMessage(patientPhoneNumber, $"Your appointment on {appointmentSlot.Date} from {appointmentSlot.StartTime} to {appointmentSlot.EndTime} with Dr. {doctor.Name} {doctor.Surname} (Contact: {doctor.Email}) has been Accepted.");
                }

                _common.AddInformationMessage("Appointment Accepted Successfully!");
                return _operationResult.SuccessResult("Appointment Accepted Successfully!");
            }
            catch (Exception exception)
            {
                _common.AddErrorMessage($"Error accepting Appointment: {exception.Message}");
                return _operationResult.ErrorResult($"Failed to accept Appointment:", new[] { exception.Message });
            }
        }

        public async Task<OperationResult> DeclineAppointment(string id)
        {
            try
            {
                var appointments = await GetAllAppointments();
                var existingAppointment = appointments.FirstOrDefault(a => a.BookAppointmentId == id);
                if (existingAppointment != null)
                {
                    return _operationResult.ErrorResult($"Failed to decline Appointment: Appointment with ID {id} already exists.", new[] { "Appointment already exists." });
                }

                var bookAppointment = await _bookappointmentRepository.GetByIdAsync(id);
                if (bookAppointment == null)
                {
                    return _operationResult.ErrorResult($"Failed to decline Book Appointment: Book Appointment with ID {id} not found.", new[] { "Book Appointment not found." });
                }
                var appointment = new Appointment
                {
                    BookAppointmentId = id,
                    Status = AppointmentStatus.Declined.ToString()
                };

                var result = await _appointmentRepository.AddAsync(appointment);

                if (!result.Succeeded)
                {
                    return _operationResult.ErrorResult($"Failed to Decline Appointment:", result.Errors);
                }

                bookAppointment.BookAppointmentStatus = AppointmentStatus.Declined.ToString();
                await _bookappointmentRepository.UpdateAsync(bookAppointment);

                var appointmentSlot = await _appointmentSlotRepository.GetByIdAsync(bookAppointment.AppointmentSlotId);
                if (appointmentSlot != null)
                {
                    appointmentSlot.IsBooked = false;
                    await _appointmentSlotRepository.UpdateAsync(appointmentSlot);
                }
                
                var doctor = await _doctorRepository.GetByIdAsync(appointmentSlot.DoctorId);

                var patientId = bookAppointment.PatientId;

                // Fetch the phone number associated with the patientId from your repository or service
                var patientPhoneNumber = await _patientRepository.GetPhoneNumberByIdAsync(patientId);
                // Check if the phone number is not null or empty before sending the message
                if (!string.IsNullOrEmpty(patientPhoneNumber))
                {
                    _twilioService.SendMessage(patientPhoneNumber, $"Your appointment on {appointmentSlot.Date} from {appointmentSlot.StartTime} to {appointmentSlot.EndTime} with Dr. {doctor.Name} {doctor.Surname} (Contact: {doctor.Email}) has been Declined.");
                }

                _common.AddInformationMessage("Appointment Declined Successfully!");
                return _operationResult.SuccessResult("Appointment Declined Successfully!");
            }
            catch (Exception exception)
            {
                _common.AddErrorMessage($"Error accepting Appointment: {exception.Message}");
                return _operationResult.ErrorResult($"Failed to accept Appointment:", new[] { exception.Message });
            }
        }

        public async Task<OperationResult> DoctorCancelAppointment(string id)
        {
            try
            {
                var appointment = await _appointmentRepository.GetByIdAsync(id);
                if (appointment == null)
                {
                    return _operationResult.ErrorResult($"Failed to cancel  Appointment: Appointment with ID {id} not found.", new[] { "Appointment not found." });
                }

                if (appointment.Status == AppointmentStatus.Canceled.ToString())
                {
                    return _operationResult.ErrorResult($"Failed to cancel  Appointment: Appointment with ID {id} is already canceled.", new[] { "Appointment is already canceled." });
                }

                appointment.Status = AppointmentStatus.Canceled.ToString();
                await _appointmentRepository.UpdateAsync(appointment);

                var bookAppointment = await _bookappointmentRepository.GetByIdAsync(appointment.BookAppointmentId);
                if (bookAppointment == null)
                {
                    return _operationResult.ErrorResult($"Failed to cancel  Appointment: Book Appointment with ID {appointment.BookAppointmentId} not found.", new[] { "Book Appointment not found." });
                }

                if (bookAppointment.BookAppointmentStatus == AppointmentStatus.Canceled.ToString())
                {
                    return _operationResult.ErrorResult($"Failed to cancel  Appointment: Book Appointment with ID {appointment.BookAppointmentId} is already canceled.", new[] { "Book Appointment is already canceled." });
                }

                bookAppointment.BookAppointmentStatus = AppointmentStatus.Canceled.ToString();
                await _bookappointmentRepository.UpdateAsync(bookAppointment);

                //var appointmentSlot = await _appointmentSlotRepository.GetByIdAsync(bookAppointment.AppointmentSlotId);
                //if (appointmentSlot != null)
                //{
                //    appointmentSlot.IsBooked = false;
                //    await _appointmentSlotRepository.UpdateAsync(appointmentSlot);
                //}
                var appointmentSlot = await _appointmentSlotRepository.GetByIdAsync(bookAppointment.AppointmentSlotId);
                var doctor = await _doctorRepository.GetByIdAsync(appointmentSlot.DoctorId);

                var patientId = bookAppointment.PatientId;

                // Fetch the phone number associated with the patientId from your repository or service
                var patientPhoneNumber = await _patientRepository.GetPhoneNumberByIdAsync(patientId);
                // Check if the phone number is not null or empty before sending the message
                if (!string.IsNullOrEmpty(patientPhoneNumber))
                {
                    _twilioService.SendMessage(patientPhoneNumber, $"Your appointment on {appointmentSlot.Date} from {appointmentSlot.StartTime} to {appointmentSlot.EndTime} with Dr. {doctor.Name} {doctor.Surname} (Contact: {doctor.Email}) has been Canceled.");
                }
                _common.AddInformationMessage("Appointment and Book Appointment canceled successfully.");
                return _operationResult.SuccessResult("Appointment and Book Appointment canceled successfully.");
            }
            catch (Exception exception)
            {
                _common.AddErrorMessage($"Error canceling Appointment: {exception.Message}");
                return _operationResult.ErrorResult($"Failed to cancel Appointment:", new[] { exception.Message });
            }
        }

        public async Task<OperationResult> PatientCancelAppointment(string id)
        {
            try
            {
                var appointment = await _appointmentRepository.GetByIdAsync(id);
                if (appointment == null)
                {
                    return _operationResult.ErrorResult($"Failed to cancel  Appointment: Appointment with ID {id} not found.", new[] { "Appointment not found." });
                }

                if (appointment.Status == AppointmentStatus.Canceled.ToString())
                {
                    return _operationResult.ErrorResult($"Failed to cancel  Appointment: Appointment with ID {id} is already canceled.", new[] { "Appointment is already canceled." });
                }

                appointment.Status = AppointmentStatus.Canceled.ToString();
                await _appointmentRepository.UpdateAsync(appointment);

                var bookAppointment = await _bookappointmentRepository.GetByIdAsync(appointment.BookAppointmentId);
                if (bookAppointment == null)
                {
                    return _operationResult.ErrorResult($"Failed to cancel  Appointment: Book Appointment with ID {appointment.BookAppointmentId} not found.", new[] { "Book Appointment not found." });
                }

                if (bookAppointment.BookAppointmentStatus == AppointmentStatus.Canceled.ToString())
                {
                    return _operationResult.ErrorResult($"Failed to cancel  Appointment: Book Appointment with ID {appointment.BookAppointmentId} is already canceled.", new[] { "Book Appointment is already canceled." });
                }

                bookAppointment.BookAppointmentStatus = AppointmentStatus.Canceled.ToString();
                await _bookappointmentRepository.UpdateAsync(bookAppointment);

                var appointmentSlot = await _appointmentSlotRepository.GetByIdAsync(bookAppointment.AppointmentSlotId);
                if (appointmentSlot != null)
                {
                    appointmentSlot.IsBooked = false;
                    await _appointmentSlotRepository.UpdateAsync(appointmentSlot);
                }
                var patient = await _patientRepository.GetByIdAsync(bookAppointment.PatientId);

                var doctorId = appointmentSlot.DoctorId;
                _logger.LogInformation($"number of doctor {doctorId}");
                // Fetch the phone number associated with the patientId from your repository or service
                var doctorPhoneNumber = await _doctorRepository.GetPhoneNumberByIdAsync(doctorId);
                
                _logger.LogInformation($"doc number {doctorPhoneNumber}");
                // Check if the phone number is not null or empty before sending the message
                if (!string.IsNullOrEmpty(doctorPhoneNumber))
                {
                    _twilioService.SendMessage(doctorPhoneNumber, $"Your appointment on {appointmentSlot.Date} from {appointmentSlot.StartTime} to {appointmentSlot.EndTime} with Patient {patient.Name} {patient.Surname} (Contact: {patient.Email}) has been Canceled.");
                }
                _common.AddInformationMessage("Appointment and Book Appointment canceled successfully.");
                return _operationResult.SuccessResult("Appointment and Book Appointment canceled successfully.");
            }
            catch (Exception exception)
            {
                _common.AddErrorMessage($"Error canceling Appointment: {exception.Message}");
                return _operationResult.ErrorResult($"Failed to cancel Appointment:", new[] { exception.Message });
            }
        }

        public async Task<OperationResult> PatientCancelAppointmentPostMethod(string id)
        {
            try
            {
                var appointments = await GetAllAppointments();
                var existingAppointment = appointments.FirstOrDefault(a => a.BookAppointmentId == id);
                if (existingAppointment != null)
                {
                    return _operationResult.ErrorResult($"Failed to cancel Appointment: Appointment with ID {id} already exists.", new[] { "Appointment already exists." });
                }

                var bookAppointment = await _bookappointmentRepository.GetByIdAsync(id);
                if (bookAppointment == null)
                {
                    return _operationResult.ErrorResult($"Failed to cancel Book Appointment: Book Appointment with ID {id} not found.", new[] { "Book Appointment not found." });
                }

                if (bookAppointment.BookAppointmentStatus == AppointmentStatus.Canceled.ToString())
                {
                    return _operationResult.ErrorResult($"Failed to cancel Appointment: Book Appointment with ID {id} is already canceled.", new[] { "Book Appointment is already canceled." });
                }

                // Create a new Appointment with the provided bookAppointmentId and set its status to "Canceled"
                var appointment = new Appointment
                {
                    BookAppointmentId = id,
                    Status = AppointmentStatus.Canceled.ToString()
                };

                var result = await _appointmentRepository.AddAsync(appointment);

                if (!result.Succeeded)
                {
                    return _operationResult.ErrorResult($"Failed to cancel Appointment:", result.Errors);
                }

                bookAppointment.BookAppointmentStatus = AppointmentStatus.Canceled.ToString();
                await _bookappointmentRepository.UpdateAsync(bookAppointment);

                var appointmentSlot = await _appointmentSlotRepository.GetByIdAsync(bookAppointment.AppointmentSlotId);
                if (appointmentSlot != null)
                {
                    appointmentSlot.IsBooked = false;
                    await _appointmentSlotRepository.UpdateAsync(appointmentSlot);
                }

                _common.AddInformationMessage("Appointment Canceled Successfully!");
                return _operationResult.SuccessResult("Appointment Canceled Successfully!");
            }
            catch (Exception exception)
            {
                _common.AddErrorMessage($"Error canceling Appointment: {exception.Message}");
                return _operationResult.ErrorResult($"Failed to cancel Appointment:", new[] { exception.Message });
            }
        }

    }
}
