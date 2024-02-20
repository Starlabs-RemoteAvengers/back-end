﻿using AppointEase.Application.Contracts.Identity;
using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.DbModels;
using AppointEase.Data.Data;
using AppointEase.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Data
{
    public static class DataInjectionServices
    {
        public static void AddDataServices(this IServiceCollection serviceDescriptors, IConfiguration configuration)
        {
            serviceDescriptors.AddDbContext<AppointEaseContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")
                );
            });
            

            serviceDescriptors.AddScoped<AppointEaseContext>();
            serviceDescriptors.AddScoped<IRepository<TblPacient>, UserRepository>();
            serviceDescriptors.AddScoped<IRepository<TblAdmin>, AdminRepository>();
            serviceDescriptors.AddScoped<IRepository<TblClinic>, ClinicRepository>();
            serviceDescriptors.AddScoped<IRepository<TblDoctor>, DoctorReporsitory>();

        }
    }
}
