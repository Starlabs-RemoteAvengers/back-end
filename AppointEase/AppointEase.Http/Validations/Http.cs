using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointEase.Http.Contracts.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AppointEase.Http.Contracts;
using AppointEase.Http.Services;
using Refit;

namespace AppointEase.Http.Contracts
{
    public class Http
    {
        public static void AddHttpModule(IServiceCollection serviceDescriptors, IConfiguration configuration)
        {
            serviceDescriptors.AddScoped<IStripeApi, StripeService>();
            serviceDescriptors.AddRefitClient<IStripeApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.stripe.com/api"));
        }
    }
}
