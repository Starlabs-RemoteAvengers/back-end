using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Application.Services;
using AppointEase.Data.Contracts.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointEase.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookAppointmentController : ControllerBase
    {
        private readonly IBookAppointmentService _bookappointmentService;
        private readonly IValidator<BookAppointmentRequest> _bookappointmentValidator;
        public BookAppointmentController(IBookAppointmentService bookappointmentService, IValidator<BookAppointmentRequest> bookappointmentValidator)
        {
            _bookappointmentService = bookappointmentService;
            _bookappointmentValidator = bookappointmentValidator;
        }

        [HttpPost("CreateBookAppointment")]
        public async Task<IActionResult> CreateBookAppointment([FromBody] BookAppointmentRequest bookappointmentRequest)
        {
            _bookappointmentValidator.ValidateAndThrow(bookappointmentRequest);

            var result = await _bookappointmentService.CreateBookAppointment(bookappointmentRequest);
            return Ok(result);
        }
        //[HttpPut("Accept/{id}")]
        //public async Task<IActionResult> AcceptAppointment(string id)
        //{
        //    var operationResult = await _bookappointmentService.AcceptBookAppointment(id);

        //    return Ok(operationResult);
        //}
        [HttpGet]
        public async Task<IActionResult> GetAllBookAppointment()
        {
            var bookappointment = await _bookappointmentService.GetAllBookAppointment();
            return Ok(bookappointment);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookAppointmentById(string id)
        {
            var bookappointment = await _bookappointmentService.GetBookAppointmentById(id);
            return Ok(bookappointment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBookAppointment(string id, [FromBody] BookAppointmentRequest bookappointmentRequest)
        {
            await _bookappointmentService.UpdateBookAppointment(id, bookappointmentRequest);
            return Ok("BookAppointment updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookAppointment(string id)
        {
            await _bookappointmentService.DeleteBookAppointment(id);
            return Ok("BookAppointment deleted successfully");
        }

        //[HttpPut("cancel/{id}")]
        //public async Task<IActionResult> CancelAppointment(string id)
        //{
        //    try
        //    {
        //        var result = await _bookappointmentService.CancelBookAppointment(id);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

        //[HttpPut("cancel-from-patient/{id}")]
        //public async Task<IActionResult> CancelAppointmentFromPatient(string id)
        //{
        //    try
        //    {
        //        var result = await _bookappointmentService.CancelBookAppointmentFromPatient(id);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}
    }
}
