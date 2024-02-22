﻿using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Data.Contracts.Identity;
using AppointEase.Data.Contracts.Interfaces;
using AppointEase.Data.Contracts.Models;
using AppointEase.Data.Data;
using AppointEase.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            serviceDescriptors.AddIdentity<ApplicationUser, IdentityRole>()
                 .AddEntityFrameworkStores<AppointEaseContext>()
                 .AddDefaultTokenProviders();

            serviceDescriptors.AddScoped<AppointEaseContext>();
            serviceDescriptors.AddScoped<IRepository<TblPacient>, UserRepository>();
            serviceDescriptors.AddScoped<IRepository<TblAdmin>, AdminRepository>();
            serviceDescriptors.AddScoped<IRepository<TblClinic>, ClinicRepository>();
            serviceDescriptors.AddScoped<IRepository<TblDoctor>, DoctorReporsitory>();

        }
    }
}
