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
            CreateMap<Admin, AdminRequest>().ReverseMap().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Doctor, DoctorRequest>().ReverseMap().ReverseMap().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<ApplicationUser, PatientRequest>().ReverseMap();
            CreateMap<Clinic, ApplicationUser>();
            CreateMap<DoctorRequest, ApplicationUser>().ReverseMap();
            CreateMap<Clinic, ClinicRequest>().ReverseMap().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<ApplicationUser, PatientRequest>().ReverseMap();
            CreateMap<Patient, ApplicationUser>().ReverseMap();
            CreateMap<Clinic, ApplicationUser>().ReverseMap();
            CreateMap<ApplicationUser, AdminRequest>().ReverseMap();
        }
    }
}
