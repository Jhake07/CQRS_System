using CleanArchitectureSystem.Application.Contracts.Identity;
using CleanArchitectureSystem.Application.Exceptions;
using CleanArchitectureSystem.Application.Models.Identity;
using CleanArchitectureSystem.Application.Response;
using CleanArchitectureSystem.Identity.EntityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArchitectureSystem.Identity.Services
{
    public class AppAuthService(UserManager<AppUser> userManager, IOptions<JwtSettings> jwtSettings, SignInManager<AppUser> signInManager, ILogger<AppAuthService> logger) : IAppAuthServiceRepository
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;
        private readonly ILogger<AppAuthService> _logger = logger; // Add logger

        public async Task<AuthResponse> Login(AuthRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                //throw new NotFoundException($"User with {request.Email} not found.", request.Email);
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = $"User with Email: {request.Email} was not found."
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded == false)
            {
                //throw new BadRequestException($"Credentials for '{request.Email} aren't valid'.");
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = $"Credentials for '{request.Email} aren't valid'.",
                    Id = request.Email // Ensure the ID is converted to string if necessary
                };
            }

            JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

            var response = new AuthResponse
            {
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                UserName = user.UserName
            };

            return response;
        }

        private async Task<JwtSecurityToken> GenerateToken(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
               issuer: _jwtSettings.Issuer,
               audience: _jwtSettings.Audience,
               claims: claims,
               expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
               signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        public async Task<IdentityResultResponse> Register(RegistrationRequest request)
        {

            try
            {
                var user = new AppUser
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    UserName = request.UserName,
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    var errorMessages = string.Join("\n", result.Errors.Select(err => $"• {err.Description}"));
                    throw new BadRequestException(errorMessages);
                }

                await _userManager.AddToRoleAsync(user, "User");

                return new IdentityResultResponse
                {
                    IsSuccess = true,
                    Message = "User registered successfully",
                    Id = user.Id,
                    Email = user.Email
                };
            }
            catch (Exception ex)
            {
                // Log the error (example: using ILogger)
                _logger.LogError(ex, "Error registering user");

                throw new ApplicationException("An unexpected error occurred while registering the user. Please try again.");
            }

        }

        public Task<CustomResultResponse> UpdateUserAccount(UpdateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
