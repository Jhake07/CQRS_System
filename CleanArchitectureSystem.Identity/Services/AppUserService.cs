using CleanArchitectureSystem.Application.Contracts.Identity;
using CleanArchitectureSystem.Application.DTO;
using CleanArchitectureSystem.Identity.EntityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CleanArchitectureSystem.Identity.Services
{
    public class AppUserService : IAppUserServiceRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        public AppUserService(UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        public string UserId { get => _contextAccessor.HttpContext?.User?.FindFirstValue("uid"); }

        public async Task<AppUserDto> GetUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);

            return MapToAppUserDto(user, roles);

        }

        public async Task<List<AppUserDto>> GetUsers()
        {
            var users = _userManager.Users.ToList();

            var userDtoList = new List<AppUserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtoList.Add(MapToAppUserDto(user, roles));
            }

            return userDtoList;

        }
        private static AppUserDto MapToAppUserDto(AppUser user, IList<string> roles)
        {
            return new AppUserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate,
                Username = user.UserName,
                Role = roles?.FirstOrDefault() ?? string.Empty
            };
        }

    }
}
