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

        public virtual DbSet<Admin> Admin { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public virtual DbSet<Clinic> Clinic { get; set; }
        public virtual DbSet<Doctor> Doctor { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=AppointEase;Integrated Security=True; TrustServerCertificate=true;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Admin>().HasKey(e => e.AdminId);
            builder.Entity<Doctor>().HasKey(e => e.Id);
            builder.Entity<Clinic>().HasKey(e => e.Id);
            builder.Entity<Patient>().ToTable("Patient");

            // No need to configure the key for Patient if it inherits from ApplicationUser

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
