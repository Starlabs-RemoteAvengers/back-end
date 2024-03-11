using AppointEase.Application.Contracts.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Interfaces
{
    public interface ISearchService
    {
        Task<List<object>> GetSerach(SearchRequest searchRequest);
        Task<IEnumerable<object>> GetAllDoctors();
    }
}
