using AppointEase.Application.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Data.Data
{
    public class DataContext : DbContext
    {
        private readonly ILogger<DataContext> _logger;
        private readonly IConfiguration _configuration;
        public DataContext(DbContextOptions<DataContext> options, ILogger<DataContext> logger, IConfiguration configuration) : base(options)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public DbSet<Person> Person { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
