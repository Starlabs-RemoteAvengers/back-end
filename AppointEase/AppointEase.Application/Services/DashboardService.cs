using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Data.Contracts.Models;

namespace AppointEase.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IAppointmentSlotService _slotService;
        private readonly IBookAppointmentService _bookAppointment;
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorService doctorService;
        private readonly IPatientService patientService;
        private readonly IOperationResult _operationResult;
        private readonly IConnectionUserService connectionUserService;

        public DashboardService(IAppointmentSlotService appointmentSlotService, IOperationResult operationResult, 
            IBookAppointmentService bookAppointment, IAppointmentService AppointmentService, IDoctorService doctorService, IPatientService patientService,IConnectionUserService connectionUserService)
        {
            this._slotService = appointmentSlotService;
            _operationResult = operationResult;
            _bookAppointment = bookAppointment;
            _appointmentService = AppointmentService;
            this.doctorService = doctorService;
            this.patientService = patientService;
            this.connectionUserService = connectionUserService;

        }

        public async Task<object> GetDashboardForPatient(string userId)
        {
            try
            {
                var random = new Random();

                var appointments =  await _appointmentService.GetAllAppointments();
                var bookAppointments = await _bookAppointment.GetAllBookAppointment();

                var selectedDoctors = (await doctorService.GetAllDoctors())
                    .OrderBy(_ => random.Next())
                    .Take(5);

                var selectedPatients = (await patientService.GetAllPatients()).Where(x=> x.Id != userId)
                    .OrderBy(_ => random.Next())
                    .Take(5);

                var allConnecitons = await connectionUserService.GetAllConnections();


                var slots = await _slotService.GetAllAppointmentSlots();
            

                var bookIds = bookAppointments.Where(x => x.PatientId == userId)
                                   .Select(x => x.BookAppointmentId)
                                   .ToList();
                var appointmentSlotIds = bookAppointments.Where(x => x.PatientId == userId)
                                 .Select(x => x.AppointmentSlotId)
                                 .ToList();
                var totalAppointmentsforUser = appointments.Count(appointment =>
                {
                    return bookIds.Contains(appointment.BookAppointmentId);
                });
                await Console.Out.WriteLineAsync("Total: "+totalAppointmentsforUser.ToString());
                var totalCompletedAppointmentsForUser = appointments.Count(appointment =>
                                        appointment.Status == "Accepted" &&
                                        bookIds.Contains(appointment.BookAppointmentId));

                var totalPendingAppointmentsForUser = bookAppointments.Count(appointmentSlotId =>
                                                      appointmentSlotId.BookAppointmentStatus == "Pending" &&
                                                     bookIds.Contains(appointmentSlotId.BookAppointmentId)
                                                  );

                var totalFreeSlots = slots.Count(slot => slot.IsBooked.Equals(false));


                #region AppointmentsTable
                var tasks = appointmentSlotIds.Select(async x =>
                {
                    var slotExists = slots.Any(s => s.AppointmentSlotId == x);
                    var slot = slots.FirstOrDefault(s => s.AppointmentSlotId == x);
                    if (slot == null)
                        return null;

                    var bookId = bookAppointments.FirstOrDefault(b => b.AppointmentSlotId.Equals(x));
                    var doctor = await doctorService.GetDoctor(slot.DoctorId);

                    if (slotExists && bookId != null && doctor != null)
                    {
                        return new
                        {
                            AppointmentSlotId = bookId.BookAppointmentId,
                            Date = slot.Date,
                            StartTime = slot.StartTime,
                            EndTime = slot.EndTime,
                            Doctor = doctor.Name + " " + doctor.Surname,
                            Meetingreason = bookId.MeetingReason,
                            MeetingRequest = bookId.MeetingRequestDescription
                        };
                    }
                    else
                    {
                        return null;
                    }
                }).ToList();

                await Task.WhenAll(tasks);

                var tableAppointments = tasks.Take(5);
                #endregion

                #region Doctors
                var listOfDoctors = selectedDoctors.Select(doctor =>
                {
                    return new
                    {
                        DoctorId = doctor.Id,
                        FullName = doctor.Name + " " + doctor.Surname,
                        Specialisation = doctor.Specialisation,
                        PhotoData = doctor.PhotoData,
                        PhotoFormat = doctor.PhotoFormat,
                        isFriends = allConnecitons.Any(x=> x.FromId == userId && x.ToId == doctor.Id || x.FromId == doctor.Id && x.ToId == userId)
                    };
                }).ToList();
                #endregion

                #region Patients
                var listOfPatients = selectedPatients.Select(patient =>
                {
                    return new
                    {
                        PatientId = patient.Id,
                        FullName = patient.Name + " " + patient.Surname,
                        PhotoData = patient.PhotoData,
                        PhotoFormat = patient.PhotoFormat,
                        isFriends = allConnecitons.Any(x => x.FromId == userId && x.ToId == patient.Id || x.FromId == patient.Id && x.ToId == userId)
                    };
                }).ToList();
                #endregion



                return new
                {
                    totalAppointments = totalAppointmentsforUser,
                    totalCompleted = totalCompletedAppointmentsForUser,
                    totalFreeSlots = totalFreeSlots,
                    totalPending = totalPendingAppointmentsForUser,
                    tableAppointments = tableAppointments,
                    SugestionDoctors = listOfDoctors,
                    SugestionPatients = listOfPatients
                };

            }
            catch (Exception ex)
            {
                return _operationResult.ErrorResult($"Failed :", new[] { ex.Message });
            }
        }
    }
}
