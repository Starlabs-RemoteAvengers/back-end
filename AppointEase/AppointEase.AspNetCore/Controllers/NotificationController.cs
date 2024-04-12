using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointEase.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            this._notificationService = notificationService;
        }
        [HttpGet("Notificatoin/user")]
        public async Task<IEnumerable<object>> GetNotification([FromQuery]string id)
        {
            var result = await _notificationService.GetSpecificNotifications(id);
            return result;
        }

        [HttpPut("Notificatoin/update")]
        public async Task<OperationResult> UpdateNotification([FromBody]NotificationRequest notificationRequest, [FromQuery]string id)
        {
            var result = await _notificationService.UpdateNotification(id, notificationRequest);
            return result;
        }
        [HttpDelete("Notificatoin/delete")]
        public async Task<OperationResult> UpdateNotification([FromQuery] string[] id)
        {
            await Console.Out.WriteLineAsync(id.FirstOrDefault());
            var result = await _notificationService.DeleteNotification(id);
            return result;
        }
    }
}
