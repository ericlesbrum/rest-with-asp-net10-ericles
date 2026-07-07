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
        return Ok();
    }

    [HttpGet("Id")]
    public ActionResult Get(long Id)
    {
        return Ok();
    }

    [HttpPost]
    public ActionResult Post(Person person)
    {
        return Ok();
    }

    [HttpPut]
    public ActionResult Put(Person person)
    {
        return Ok();
    }

    [HttpDelete]
    public ActionResult Delete(long Id)
    {
        return Ok();
    }
}
