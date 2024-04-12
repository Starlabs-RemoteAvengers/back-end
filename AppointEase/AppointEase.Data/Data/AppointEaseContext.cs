using AppointEase.Data.Contracts.Identity;
using AppointEase.Data.Contracts.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppointEase.Data.Data
{
    public class AppointEaseContext : IdentityDbContext<ApplicationUser>
    {
        public AppointEaseContext()
        {
        }

        public AppointEaseContext(DbContextOptions<AppointEaseContext> options)
            : base(options)
        {
        }

        public DbSet<Admin> Admin { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Clinic> Clinic { get; set; }
        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentSlot> AppointmentSlot { get; set; }
        public DbSet<BookAppointment> BookAppointment { get; set; }
        public DbSet<ChatMessages> Messages { get; set; }
        public DbSet<ConnectionRequests> ConnectionRequests { get; set; }
        public DbSet<Connections> Connections { get; set; }
        public DbSet<Notifications> Notifications { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-EUO8BVU\\MSSQLSERVER01;Initial Catalog=AppointEase;Integrated Security=True; TrustServerCertificate=true;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Admin>().ToTable("Admin");
            builder.Entity<Doctor>().ToTable("Doctor");
            builder.Entity<Clinic>().ToTable("Clinic");
            builder.Entity<Patient>().ToTable("Patient");
            builder.Entity<AppointmentSlot>().ToTable("AppointmentSlot");
            builder.Entity<BookAppointment>().ToTable("BookAppointment");
            builder.Entity<Appointment>().ToTable("Appointments");

            SeedRoles(builder);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole() { Name = "Clinic", ConcurrencyStamp = "2", NormalizedName = "Clinic" },
                new IdentityRole() { Name = "Doctor", ConcurrencyStamp = "3", NormalizedName = "Doctor" },
                new IdentityRole() { Name = "Patient", ConcurrencyStamp = "4", NormalizedName = "Patient" });
        }
    }

}
