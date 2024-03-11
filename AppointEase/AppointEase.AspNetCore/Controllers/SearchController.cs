using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointEase.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;
        private readonly IApplicationExtensions _appExtensions;

        public SearchController(ISearchService searchService, IApplicationExtensions applicationExtensions)
        {
            this._searchService = searchService;
            this._appExtensions = applicationExtensions;
        }
        [HttpGet("SearchAppointment")]
        public async Task<IActionResult> SearchAppointment([FromQuery] SearchRequest searchRequest)
        {
            try
            {
                
                var searchResults = await _searchService.GetSerach(searchRequest);
                return Ok(searchResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during search: {ex.Message}");
            }
        }
        [HttpGet("FetchAllDoctors")]
        public async Task<IActionResult> FetchAllDoctors()
        {
            try
            {
                var searchResults = await _searchService.GetAllDoctors();
                return Ok(searchResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during search: {ex.Message}");
            }
        }
    }
}
 