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
            CreateMap<TblAdmin, AdminRequest>().ReverseMap();
            CreateMap<TblClinic, ClinicRequest>().ReverseMap();
            CreateMap<TblDoctor, DoctorRequest>().ReverseMap();
            CreateMap<TblPacient, PatientRequest>().ReverseMap();
            CreateMap<TblPacient, PatientResponse>();
            CreateMap<PatientRequest,ApplicationUser>();
          //  CreateMap<ApplicationUserRequest, ApplicationUser>();
        }
    }
}
