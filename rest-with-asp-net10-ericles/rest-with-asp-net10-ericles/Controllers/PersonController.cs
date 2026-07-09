using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rest_with_asp_net10_ericles.Model;
using rest_with_asp_net10_ericles.Services.Interfaces;

namespace rest_with_asp_net10_ericles.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;
    private readonly ILogger<PersonController> _logger;

    public PersonController(IPersonService personService, ILogger<PersonController> logger)
    {
        _personService = personService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("Fetching all persons");
        return StatusCode(StatusCodes.Status200OK, _personService.FindAll());
    }

    [HttpGet("{Id}")]
    public IActionResult Get(int Id)
    {
        _logger.LogInformation("Fetching person by ID: {Id}", Id);
        var person = _personService.FindById(Id);
        if (person == null)
        {
            _logger.LogWarning("Person with ID: {Id} not found", Id);
            return NotFound();
        }

        return StatusCode(StatusCodes.Status200OK, person);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Person person)
    {
        _logger.LogInformation("Creating new person: {firstName} {lastName}", person.FirstName, person.LastName);
        var createdPerson = _personService.Create(person);
        if (createdPerson == null)
        {
            _logger.LogError("Failed to create person: {firstName} {lastName}", person.FirstName, person.LastName);
            return BadRequest("Person could not be created.");
        }
        return StatusCode(StatusCodes.Status201Created, createdPerson);
    }

    [HttpPut]
    public IActionResult Put([FromBody] Person person)
    {
        _logger.LogInformation("Updating person with ID: {Id}", person.Id);
        var updatedPerson = _personService.Update(person);
        if (updatedPerson == null)
        {
            _logger.LogError("Failed to update person with ID: {Id}", person.Id);
            return BadRequest("Person could not be updated.");
        }
        _logger.LogDebug("Person with ID: {Id} updated successfully", person.Id);
        return StatusCode(StatusCodes.Status200OK, updatedPerson);
    }

    [HttpDelete("{Id}")]
    public IActionResult Delete(int Id)
    {
        _logger.LogInformation("Deleting person by ID: {Id}", Id);
        _personService.Delete(Id);
        _logger.LogDebug("Person with ID: {Id} deleted successfully", Id);
        return StatusCode(StatusCodes.Status204NoContent);
    }
}
