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
        public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _appAuthServiceRepository.Register(request, cancellationToken));
        }

        [HttpPut("update")]
        public async Task<ActionResult<CustomResultResponse>> UpdateAccount(string username, string email, UpdateRequest request, CancellationToken cancellationToke)
        {
            return Ok(await _appAuthServiceRepository.UpdateUserAccount(username, email, request));
        }
    }
}
