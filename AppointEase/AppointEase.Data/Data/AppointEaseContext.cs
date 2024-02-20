using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using AppointEase.Application.Contracts.Identity;
using AppointEase.Application.Contracts.Models.DbModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AppointEase.Data.Data;

public class AppointEaseContext : IdentityDbContext<ApplicationUser>
{
    public AppointEaseContext()
    {
    }

    public AppointEaseContext(DbContextOptions<AppointEaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAdmin> TblAdmins { get; set; }
    public virtual DbSet<TblPacient> TblUsers { get; set; }
    public virtual DbSet<TblClinic> TblClinic { get; set; }
    public virtual DbSet<TblDoctor> TblDoctor { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=(localdb)\\Local;Initial Catalog=AppointEase;Integrated Security=True; TrustServerCertificate=true;");
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<TblAdmin>()
        .HasKey(e => e.AdminId);

        builder.Entity<TblDoctor>()
       .HasKey(e => e.Id);

        builder.Entity<TblClinic>()
       .HasKey(e => e.Id);

        builder.Entity<TblPacient>()
       .HasKey(e => e.UserId);

        SeedRoles(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
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
