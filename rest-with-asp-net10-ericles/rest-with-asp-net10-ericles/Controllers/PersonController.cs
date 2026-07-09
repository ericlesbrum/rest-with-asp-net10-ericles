using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rest_with_asp_net10_ericles.Model;
using rest_with_asp_net10_ericles.Services.Interfaces;

namespace rest_with_asp_net10_ericles.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonController : ControllerBase
{
    private readonly IPersonService personService;

    public PersonController(IPersonService personService)
    {
        this.personService = personService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return StatusCode(StatusCodes.Status200OK, personService.FindAll());
    }

    [HttpGet("{Id}")]
    public IActionResult Get(int Id)
    {
        var person = personService.FindById(Id);
        if (person == null)
            return NotFound();

        return StatusCode(StatusCodes.Status200OK, person);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Person person)
    {
        var createdPerson = personService.Create(person);
        return StatusCode(StatusCodes.Status201Created, createdPerson);
    }

    [HttpPut]
    public IActionResult Put([FromBody] Person person)
    {
        var updatedPerson = personService.Update(person);
        return StatusCode(StatusCodes.Status200OK, updatedPerson);
    }

    [HttpDelete("{Id}")]
    public IActionResult Delete(int Id)
    {
        personService.Delete(Id);
        return StatusCode(StatusCodes.Status204NoContent);
    }
}
