﻿using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Mapper;
using AppointEase.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using AppointEase.Application.Contracts.Validator;
using AppointEase.Application.Contracts.Common;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Data.Contracts.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AppointEase.Data.Data;
using Microsoft.Extensions.Configuration;

namespace AppointEase.Application
{
    public static class ApplicationInjection
    {
        public static void AddApplicationServices(this IServiceCollection serviceDescriptors)
        {
            // AutoMapper
            serviceDescriptors.AddAutoMapper(typeof(MappingProfile));

            // FluentValidation
            serviceDescriptors.AddTransient<IValidator<PatientRequest>, CreatePatientValidator>();

            // Application services
            serviceDescriptors.AddScoped<IApplicationExtensions, ApplicationExtensions>();
            serviceDescriptors.AddScoped<IPatientService, PatientService>();
            serviceDescriptors.AddSingleton<IOperationResult, OperationResult>();
           
            // Identity services
            //serviceDescriptors.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<AppointEaseContext>()
            //    .AddDefaultTokenProviders();

        }
    }
}