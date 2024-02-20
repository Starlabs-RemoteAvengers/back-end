using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Mapper;
using AppointEase.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using AppointEase.Application.Contracts.Validator;
using AppointEase.Application.Contracts.Common;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Application.Contracts.Models.Operations;

namespace AppointEase.Application
{
    public static class ApplicationInjection
    {
        public static void AddApplicationServices(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddAutoMapper(typeof(MappingProfile));
            serviceDescriptors.AddTransient<IValidator<PatientRequest>, CreatePatientValidator>();
            serviceDescriptors.AddScoped<IApplicationExtensions, ApplicationExtensions>();
            serviceDescriptors.AddScoped<IPatientService, PatientService>();
            serviceDescriptors.AddSingleton<IOperationResult, OperationResult>();
        }
    }
}
