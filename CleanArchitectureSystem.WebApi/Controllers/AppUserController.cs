using CleanArchitectureSystem.Application.Features.AppUser.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanArchitectureSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        // GET: api/<AppUser>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AppUser>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AppUser>
        [HttpPost]
        public async Task<ActionResult> Post(CreateAppUserCommand appUser)
        {
            var response = await _mediator.Send(appUser);
            return CreatedAtAction(nameof(Get), new { id = response });
        }

        // PUT api/<AppUser>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AppUser>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
