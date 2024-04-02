using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Data.Contracts.Identity;
using AppointEase.Data.Contracts.Models;
using AutoMapper;

namespace AppointEase.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientRequest>().ReverseMap().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Admin, AdminRequest>().ReverseMap();
            CreateMap<Doctor, DoctorRequest>().ReverseMap().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<ApplicationUser, PatientRequest>().ReverseMap();
            CreateMap<Clinic, ApplicationUser>();
            CreateMap<DoctorRequest, ApplicationUser>().ReverseMap();
            CreateMap<Clinic, ClinicRequest>().ReverseMap().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<ApplicationUser, PatientRequest>().ReverseMap();
            CreateMap<Patient, ApplicationUser>().ReverseMap();
            CreateMap<Clinic, ApplicationUser>().ReverseMap();
            CreateMap<ApplicationUser, AdminRequest>().ReverseMap();
            CreateMap<AppointmentSlot, AppointmentSlotRequest>().ReverseMap().ForMember(dest => dest.AppointmentSlotId, opt => opt.Ignore());
            CreateMap<BookAppointment, BookAppointmentRequest>().ReverseMap().ForMember(dest => dest.BookAppointmentId, opt => opt.Ignore());
            CreateMap<ApplicationUser, ApplicationUserRequest>().ReverseMap().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Appointment, AppointmentRequest>().ReverseMap().ForMember(dest => dest.AppointmentId, opt => opt.Ignore());
        }
    }
}
