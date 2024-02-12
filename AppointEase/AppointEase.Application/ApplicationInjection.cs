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

namespace AppointEase.Application
{
    public static class ApplicationInjection
    {
        public static void AddApplicationServices(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddScoped<IPersonService, PersonService>();
            serviceDescriptors.AddAutoMapper(typeof(YourMappingProfile));
            serviceDescriptors.AddTransient<IValidator<PersonDto>,PersonDtoValidator>();
        }
    }
}
