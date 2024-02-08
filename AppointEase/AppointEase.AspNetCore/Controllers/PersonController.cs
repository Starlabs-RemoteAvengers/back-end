using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models;
using AppointEase.Data.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointEase.AspNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;
      

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet("{personId}")]
        public IActionResult GetPerson(int personId)
        {
            var person = _personService.GetPerson(personId);
            return Ok(person);
        }

        [HttpGet]
        public IActionResult GetAllPersons()
        {
            var persons = _personService.GetAllPersons();
            return Ok(persons);
        }

        [HttpPost]
        public IActionResult CreatePerson([FromBody] PersonDto personDto)
        {
            try
            {
                _personService.CreatePerson(personDto);
                return Ok("Created Successfully");
            }
            catch (FluentValidation.ValidationException validationException)
            {
                return BadRequest(validationException.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in an appropriate way
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{personId}")]
        public IActionResult UpdatePerson(int personId, [FromBody] PersonDto updatedPerson)
        {
            try
            {
                _personService.UpdatePerson(personId, updatedPerson);
                return Ok("Updated Successfully");
            }
            catch (FluentValidation.ValidationException validationException)
            {
                return BadRequest(validationException.Errors);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in an appropriate way
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{personId}")]
        public IActionResult DeletePerson(int personId)
        {
            _personService.DeletePerson(personId);
            return Ok();
        }
    }
}
