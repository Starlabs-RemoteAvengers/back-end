using AppointEase.Http.Contracts.Interfaces;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Http.Contracts
{
    public class MyApiClient
    {
        private readonly IStripeApi _api;

        public MyApiClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://localhost:7207/");
            _api = RestService.For<IStripeApi>(httpClient);
        }

        // Other methods...
    }

}
