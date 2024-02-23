using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Application.Contracts.ModelsRespond;
using AppointEase.Data.Contracts.Identity;
using AppointEase.Data.Contracts.Models;
using AutoMapper;

namespace AppointEase.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Admin, AdminRequest>().ReverseMap();
            CreateMap<Clinic, ClinicRequest>().ReverseMap();
            CreateMap<Doctor, DoctorRequest>().ReverseMap();
            CreateMap<Patient, PatientRequest>().ReverseMap();
            CreateMap<Patient, PatientResponse>();
            CreateMap<PatientRequest,ApplicationUser>();
            CreateMap<Patient, ApplicationUser>();

        }
    }
}
