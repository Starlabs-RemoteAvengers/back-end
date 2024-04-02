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
using Microsoft.AspNetCore.Identity;
using AppointEase.Data.Contracts.Interfaces;
using AppointEase.Data.Contracts.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNet.SignalR;
using AutoMapper;
using AppointEase.Data.Contracts.Identity;
using System;
using AppointEase.Data.Repositories;
using AppointEase.Http.Contracts.Interfaces;
using AppointEase.Http.Services;

namespace AppointEase.Application
{
    public static class ApplicationInjection
    {
        public static void AddApplicationServices(this IServiceCollection serviceDescriptors, IConfiguration configuration)
        {
            serviceDescriptors.AddAutoMapper(typeof(MappingProfile));
            serviceDescriptors.AddTransient<IValidator<PatientRequest>, CreatePatientValidator>();
            serviceDescriptors.AddScoped<IUserService, UserService>();
            serviceDescriptors.AddTransient<IValidator<DoctorRequest>, CreateDoctorValidator>(); 
            serviceDescriptors.AddScoped<IApplicationExtensions, ApplicationExtensions>();
            serviceDescriptors.AddScoped< IDoctorService, DoctorService>();
            serviceDescriptors.AddScoped<IPatientService, PatientService>();
            serviceDescriptors.AddScoped<IApplicationExtensions, ApplicationExtensions>();
            serviceDescriptors.AddSingleton<IOperationResult, OperationResult>();
            serviceDescriptors.AddScoped<IEmailServices, EmailService>();
            serviceDescriptors.AddScoped<IClinicService, ClinicService>();
            serviceDescriptors.AddScoped<IStripeApi, StripeService>();
            serviceDescriptors.AddTransient<IValidator<ClinicRequest>, CreateClinicValidator>();
            serviceDescriptors.AddSingleton<IConfiguration>(configuration);
            serviceDescriptors.AddScoped<IAdminService, AdminService>();
            serviceDescriptors.AddScoped<ISearchService, SearchService>();
            serviceDescriptors.AddTransient<IValidator<AdminRequest>, CreateAdminValidator>();
            serviceDescriptors.AddTransient<IValidator<ClinicRequest>, UpdateClinicValidator>();
            serviceDescriptors.AddScoped<IAppointmentSlotService, AppointmentSlotService>();
            serviceDescriptors.AddTransient<IValidator<AppointmentSlotRequest>, CreateAppointmentSlotValidator>();
            serviceDescriptors.AddScoped<IBookAppointmentService, BookAppointmentService>();
            serviceDescriptors.AddScoped<IValidator<BookAppointmentRequest>, CreateBookAppointmentValidator>();
            serviceDescriptors.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            serviceDescriptors.AddTransient<IValidator<PasswordRequest>, ValidatorPasswordRequest>();
            serviceDescriptors.AddTransient<IChatMessagesService,ChatMessagesService>();

            serviceDescriptors.AddScoped<IConnectionRequestService, ConnectionRequestService>();    
            serviceDescriptors.AddScoped<IConnectionUserService, ConnectionUserService>();
            serviceDescriptors.AddScoped<IHubUserService, HubUserService>();

            serviceDescriptors.AddScoped<ChatBotService>();

            GlobalHost.DependencyResolver.Register(typeof(ChatHub), () =>
            {
                return new ChatHub(serviceDescriptors.BuildServiceProvider().GetService<IChatMessagesService>(), 
                    serviceDescriptors.BuildServiceProvider().GetService<IHubUserService>(),
                    serviceDescriptors.BuildServiceProvider().GetService<UserManager<ApplicationUser>>(),
                    serviceDescriptors.BuildServiceProvider().GetService<ChatBotService>());
            });

            serviceDescriptors.AddSignalR();

            serviceDescriptors.AddScoped<IAppointmentService, AppointmentService>();
            serviceDescriptors.AddScoped<IUserService, UserService>();
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

            var emailConfigSection = configuration.GetSection("EmailConfiguration");
            var emailConfig = new EmailConfiguration
            {
                From = emailConfigSection["From"],
                SmtpServer = emailConfigSection["SmtpServer"],
                Port = int.Parse(emailConfigSection["Port"]),
                Username = emailConfigSection["Username"],
                Password = emailConfigSection["Password"]
            };

            serviceDescriptors.AddSingleton(emailConfig);
        }
    }
}
