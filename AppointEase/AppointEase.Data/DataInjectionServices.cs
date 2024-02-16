//using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Data.Data;
//using AppointEase.Data.Repositories;
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
            serviceDescriptors.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")
                );
            });

            serviceDescriptors.AddScoped<ApplicationDbContext>();
            //serviceDescriptors.AddScoped<IPersonRepository, PersonRepository>();
        }
    }
}
