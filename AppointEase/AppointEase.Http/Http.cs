using AppointEase.Http.Contracts.Interfaces;
using AppointEase.Http.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
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
            serviceDescriptors.AddScoped<IStripeApi, StripeService>();
            serviceDescriptors.AddRefitClient<IStripeApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.stripe.com/api"));
            serviceDescriptors.AddScoped<ITwilioService, TwilioService>();
        }
    }
}
