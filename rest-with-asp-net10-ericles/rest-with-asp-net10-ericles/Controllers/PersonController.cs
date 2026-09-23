using Microsoft.AspNetCore.Mvc;
using rest_with_asp_net10_ericles.Data.DTO.V2;
using rest_with_asp_net10_ericles.Hypermedia.Utils;
using rest_with_asp_net10_ericles.Services.Interfaces;

namespace rest_with_asp_net10_ericles.Controllers;

[Route("api/[controller]/v2")]
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

    [HttpGet("{sortDirection}/{pageSize}/{page}")]
    [ProducesResponseType(200, Type = typeof(PagedSearchDTO<PersonDTO>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public IActionResult Get(
        [FromRoute] string sortDirection,
        [FromRoute] int pageSize,
        [FromRoute] int page,
        [FromQuery] string name = "")
    {
        _logger.LogInformation("Fetching persons with paged search: Name={name}, SortDirection={sortDirection}, PageSize={pageSize}, Page={page}", name, sortDirection, pageSize, page);
        return Ok(_personService.FindWithPagedSearch(name, sortDirection, pageSize, page));
    }

    [HttpGet("{Id}")]
    [ProducesResponseType(200, Type = typeof(PersonDTO))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public IActionResult Get(int Id)
    {
        _logger.LogInformation("Fetching person by ID: {Id}", Id);
        var person = _personService.FindById(Id);
        if (person == null)
        {
            _logger.LogWarning("Person with ID: {Id} not found", Id);
            return NotFound();
        }

        return Ok(person);
    }

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(PersonDTO))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public IActionResult Post([FromBody] PersonDTO person)
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
    [ProducesResponseType(200, Type = typeof(PersonDTO))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public IActionResult Put([FromBody] PersonDTO person)
    {
        _logger.LogInformation("Updating person with ID: {Id}", person.Id);
        var updatedPerson = _personService.Update(person);
        if (updatedPerson == null)
        {
            _logger.LogError("Failed to update person with ID: {Id}", person.Id);
            return BadRequest("Person could not be updated.");
        }
        _logger.LogDebug("Person with ID: {Id} updated successfully", person.Id);
        return Ok(updatedPerson);
    }

    [HttpDelete("{Id}")]
    [ProducesResponseType(204, Type = typeof(PersonDTO))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public IActionResult Delete(int Id)
    {
        _logger.LogInformation("Deleting person by ID: {Id}", Id);
        var deleted = _personService.Delete(Id);
        if (!deleted)
        {
            _logger.LogWarning("Person with ID: {Id} not found for deletion", Id);
            return NotFound();
        }
        _logger.LogDebug("Person with ID: {Id} deleted successfully", Id);
        return StatusCode(StatusCodes.Status204NoContent);
    }

    [HttpPatch("{Id}")]
    [ProducesResponseType(200, Type = typeof(PersonDTO))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public IActionResult Patch(long Id)
    {
        _logger.LogInformation("Disabling person with ID: {Id}", Id);
        var patchedPerson = _personService.Disable(Id);
        if (patchedPerson == null)
        {
            _logger.LogWarning("Person with ID: {Id} not found for disabling", Id);
            return NotFound();
        }
        return Ok(patchedPerson);
    }

    [HttpGet("find-by-name")]
    [ProducesResponseType(200, Type = typeof(List<PersonDTO>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public IActionResult GetByName([FromQuery] string firstName, [FromQuery] string lastName)
    {
        _logger.LogInformation("Fetching persons by name: {firstName} {lastName}", firstName, lastName);
        return Ok(_personService.FindByName(firstName, lastName));
    }

    [HttpPost("massCreation")]
    [ProducesResponseType(200, Type = typeof(List<PersonDTO>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> MassCreation([FromForm] FileUploadDTO file)
    {
        if (file.File == null || file.File.Length == 0)
        {
            _logger.LogWarning("No file uploaded for mass creation");
            return BadRequest("File uploaded");
        }

        _logger.LogInformation("Starting mass creation from uploaded file :{filename}", file.File);

        var persons = await _personService.MassCreationAsync(file.File);
        if (persons == null)
        {
            _logger.LogError("Mass Creation failed for file: {fileName}", file.File.FileName);
            return NoContent();
        }

        _logger.LogInformation("Mass creation completed successfully with {count} records", persons.Count);

        return Ok(persons);
    }
}
