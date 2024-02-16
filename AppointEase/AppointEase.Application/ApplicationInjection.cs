using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models;
using AppointEase.Application.Mapper;
using AppointEase.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointEase.AspNetCore.Validator;
using AppointEase.Application.Contracts.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AppointEase.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using AppointEase.Application.Contracts.ModelsDto;
using AppointEase.Application.Contracts.Validator;
using AppointEase.Application.Contracts.Co;

namespace AppointEase.Application
{
    public static class ApplicationInjection

    {
        public static void AddApplicationServices(this IServiceCollection serviceDescriptors)
        {
            //serviceDescriptors.AddScoped<IPersonService, PersonService>();
            serviceDescriptors.AddAutoMapper(typeof(YourMappingProfile));
            serviceDescriptors.AddTransient<IValidator<PatientRequest>, CreatePatientValidator>();
            serviceDescriptors.AddScoped<ICommon, Common>();
            serviceDescriptors.AddScoped<IPatientService, PatientService>();


            serviceDescriptors.AddIdentity<ApplicationUser, IdentityRole>()
                   .AddEntityFrameworkStores<AppointEaseContext>()
                   .AddDefaultTokenProviders();
        }
    }
}
