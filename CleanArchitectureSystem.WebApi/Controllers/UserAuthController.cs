using CleanArchitectureSystem.Application.Contracts.Identity;
using CleanArchitectureSystem.Application.Models.Identity;
using CleanArchitectureSystem.Application.Response;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IAppAuthServiceRepository _appAuthServiceRepository;

        public UserAuthController(IAppAuthServiceRepository appAuthServiceRepository)
        {
            _appAuthServiceRepository = appAuthServiceRepository;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(AuthRequest request)
        {
            return Ok(await _appAuthServiceRepository.Login(request));
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
        {
            return Ok(await _appAuthServiceRepository.Register(request));
        }

        [HttpPut("update")]
        public async Task<ActionResult<RegistrationResponse>> UpdateAccount(string email, UpdateRequest request)
        {
            return Ok(await _appAuthServiceRepository.UpdateUserAccount(request));
        }
    }
}
