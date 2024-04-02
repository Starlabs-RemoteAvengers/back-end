using AppointEase.Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointEase.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionsController : ControllerBase
    {
        private readonly IConnectionUserService _connectionUserService;
        public ConnectionsController(IConnectionUserService connectionUserService)
        {
            this._connectionUserService = connectionUserService;
        }

        [HttpGet("UserConnection")]
        public async Task<object> GetConnections(string userId)
        {
            var result = await _connectionUserService.GetAllConnections(userId);
            return result;
        }
        [HttpDelete("DeleteFromFriends")]
        public async Task<object> DeleteFromList([FromQuery]string userId,[FromQuery]string friendId)
        {
            var result = await _connectionUserService.DeleteConnection(userId, friendId);
            return result;
        }

    }
}
