using AppointEase.Http.Contracts.Interfaces;
using AppointEase.Http.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Http
{
    public class Http
    {
        public static void AddHttpModule(IServiceCollection serviceDescriptors, IConfiguration configuration)
        {
           serviceDescriptors.AddScoped<ITwilioService, TwilioService>();
           serviceDescriptors.AddScoped<IStripeApi, StripeService>();
        }
    }
}
