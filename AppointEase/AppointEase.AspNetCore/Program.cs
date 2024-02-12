using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using AppointEase.Data.Data;
using AppointEase.Application.Contracts.Models;
using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Services;
using AppointEase.Data.Repositories;
using AppointEase.AspNetCore.Validator;
using AppointEase.Application.Mapper;
using Microsoft.SqlServer;
using AppointEase.Application.Filters;
using AppointEase.Application;
using AppointEase.Data;

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


ApplicationInjection.AddApplicationServices(builder.Services);
DataInjectionServices.AddDataServices(builder.Services,builder.Configuration);



//builder.Services.AddTransient<IValidator<PersonDto>, PersonDtoValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
