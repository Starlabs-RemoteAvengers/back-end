using AppointEase.Application.Contracts.Interfaces;
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


            serviceDescriptors.AddScoped<IRepository<Patient>, PatientRepository>();
            serviceDescriptors.AddScoped<IRepository<Admin>, AdminRepository>();
            serviceDescriptors.AddScoped<IRepository<Clinic>, ClinicRepository>();
            serviceDescriptors.AddScoped<IRepository<Doctor>, DoctorReporsitory>();
            serviceDescriptors.AddScoped<IRepository<ChatMessages>, ChatMessagesRepository>();
            serviceDescriptors.AddScoped<IRepository<AppointmentSlot>, AppointmentSlotRepository>();
            serviceDescriptors.AddScoped<IRepository<BookAppointment>, BookAppointmentRepository>();
            serviceDescriptors.AddScoped<IRepository<ConnectionRequests>, ConnectionRequestsRepository>();
            serviceDescriptors.AddScoped<IRepository<Connections>, ConnectionRepository>();
            serviceDescriptors.AddScoped<IChatMessagesRepository, ChatMessagesRepository>();
            serviceDescriptors.AddScoped<IRepository<ApplicationUser>, UsersRepository>();
            serviceDescriptors.AddScoped<IRepository<Appointment>, AppointmentRepository>();

        }
    }
}
