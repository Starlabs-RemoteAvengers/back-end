using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointEase.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestConnectionController : ControllerBase
    {
        private readonly IConnectionRequestService _connectionRequestService;
        public RequestConnectionController(IConnectionRequestService connectionRequestService)
        {
            this._connectionRequestService = connectionRequestService;
        }
        [HttpPost("AddConnection")]
        public async Task<IActionResult> AddConnection([FromBody] SendConnectionRequest request)
        {
            var result = await _connectionRequestService.AddNewConnectionRequest(request);
            return Ok(result);
        }
        [HttpGet("CheckConnection")]
        public async Task<IActionResult> CheckConnection([FromQuery] string userId, [FromQuery] string touserId)
        {
            var result = await _connectionRequestService.CheckConnectionIfExist(userId, touserId);
            return Ok(result);
        }
        [HttpGet("GetConnections")]
        public async Task<IEnumerable<object>> GetRequestConnections([FromQuery]string userId)
        {
            var result = await _connectionRequestService.GetAllRequestForSpecificUser(userId);
            return result;
        }
        [HttpGet("GetOtherConnections")]
        public async Task<IEnumerable<object>> GetOtherConnections([FromQuery] string userId)
        {
            var result = await _connectionRequestService.GetOtherUsers(userId);
            return result;
        }


        [HttpPost("AcceptRequest")]
        public async Task<IActionResult> AcceptRequest(string Id)
        {
            var result = await _connectionRequestService.AcceptRequest(Id);
            return Ok(result);
        }
        [HttpDelete("CancelRequestConnection")]
        public async Task<IActionResult> CancelRequestConnection(string idRequest)
        {
            var result = await _connectionRequestService.CancelRequest(idRequest);
            return Ok(result);
        }
      
    }
}
