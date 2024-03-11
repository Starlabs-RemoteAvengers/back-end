using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Mapper;
using AppointEase.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using AppointEase.Application.Contracts.Validator;
using AppointEase.Application.Contracts.Common;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Application.Contracts.Models.Operations;
using Microsoft.Extensions.Configuration;
using AppointEase.Application.Contracts.Models.EmailConfig;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using AppointEase.Data.Contracts.Interfaces;
using AppointEase.Data.Contracts.Models;
using AppointEase.Data.Repositories;

namespace AppointEase.Application
{
    public static class ApplicationInjection
    {
        public static void AddApplicationServices(this IServiceCollection serviceDescriptors, IConfiguration configuration)
        {
            serviceDescriptors.AddAutoMapper(typeof(MappingProfile));
            serviceDescriptors.AddScoped<IUserService, UserService>();
            serviceDescriptors.AddTransient<IValidator<PatientRequest>, CreatePatientValidator>();
            serviceDescriptors.AddTransient<IValidator<DoctorRequest>, CreateDoctorValidator>(); 
            serviceDescriptors.AddScoped<IApplicationExtensions, ApplicationExtensions>();
            serviceDescriptors.AddScoped< IDoctorService, DoctorService>();
            serviceDescriptors.AddScoped<IPatientService, PatientService>();
            serviceDescriptors.AddScoped<IApplicationExtensions, ApplicationExtensions>();
            serviceDescriptors.AddSingleton<IOperationResult, OperationResult>();
            serviceDescriptors.AddScoped<IEmailServices, EmailService>();
            serviceDescriptors.AddScoped<IClinicService, ClinicService>();
            serviceDescriptors.AddTransient<IValidator<ClinicRequest>, CreateClinicValidator>();
            serviceDescriptors.AddSingleton<IConfiguration>(configuration);
            serviceDescriptors.AddScoped<IAdminService, AdminService>();
            serviceDescriptors.AddTransient<IValidator<AdminRequest>, CreateAdminValidator>();
            serviceDescriptors.AddTransient<IValidator<ClinicRequest>, UpdateClinicValidator>();
            serviceDescriptors.AddScoped<IAppointmentSlotService, AppointmentSlotService>();
            serviceDescriptors.AddTransient<IValidator<AppointmentSlotRequest>, CreateAppointmentSlotValidator>();
            serviceDescriptors.AddScoped<IRepository<AppointmentSlot>, AppointmentSlotRepository>();
            serviceDescriptors.AddScoped<IBookAppointmentService, BookAppointmentService>();
            serviceDescriptors.AddScoped<IValidator<BookAppointmentRequest>, CreateBookAppointmentValidator>();
            serviceDescriptors.AddScoped<IRepository<BookAppointment>, BookAppointmentRepository>();
            serviceDescriptors.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            serviceDescriptors.AddTransient<IValidator<PasswordRequest>, ValidatorPasswordRequest>();
            serviceDescriptors.AddScoped<IUrlHelper>(serviceProvider =>
            {
                var actionContext = serviceProvider.GetRequiredService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });


            serviceDescriptors.Configure<IdentityOptions>(
                opt => opt.SignIn.RequireConfirmedEmail = true);


            serviceDescriptors.AddHttpContextAccessor();
            serviceDescriptors.Configure<IdentityOptions>(
                opt => opt.SignIn.RequireConfirmedEmail = true);


            serviceDescriptors.AddScoped<IEmailServices, EmailService>();


            var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            serviceDescriptors.AddSingleton(emailConfig);
        }
    }
}
