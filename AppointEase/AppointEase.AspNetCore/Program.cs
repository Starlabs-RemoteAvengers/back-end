using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using AppointEase.Data.Data;
//using AppointEase.Application.Contracts.Models;
//using AppointEase.Application.Contracts.Interfaces;
//using AppointEase.Application.Services;
//using AppointEase.Data.Repositories;
//using AppointEase.AspNetCore.Validator;
using AppointEase.Application.Mapper;
using Microsoft.SqlServer;
using AppointEase.Application.Filters;
using AppointEase.Application;
using AppointEase.Data;
using Microsoft.AspNetCore.Identity;
using AppointEase.Application.Contracts.Models;
using AppointEase.Data.Repository.IRepository;
using AppointEase.Data.Repository;
using Microsoft.AspNetCore.Identity.UI.Services;
using AppointEase.Application.Contracts.Constants;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add configuration
builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});

// Set the default culture to invariant
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;


//ApplicationInjection.AddApplicationServices(builder.Services);
DataInjectionServices.AddDataServices(builder.Services,builder.Configuration);



//builder.Services.AddTransient<IValidator<PersonDto>, PersonDtoValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationDbContext>(options => 
     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>() 
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();

app.Run();
