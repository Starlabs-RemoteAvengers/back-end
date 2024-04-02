using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace AppointEase.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IChatMessagesService _chatMessagesService;

        public ChatController(IHubContext<ChatHub> hubContext,IChatMessagesService chatMessagesService)
        {
            _hubContext = hubContext;
            _chatMessagesService = chatMessagesService;
        }

        [HttpGet("messages")]
        public async Task<IActionResult> GetMessages(string senderId, string receiverId)
        {
            try
            {
                var messages = await _chatMessagesService.GetMessagesAsync(senderId, receiverId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost]
        [Route("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] MessageRequest messageRequest)
        {
            try
            {
                await _chatMessagesService.SendMessageAsync(messageRequest);

                return Ok("Message sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while sending the message.");
            }
        }
    }


}
