using CleanArchitectureSystem.Application.Contracts.Identity;
using CleanArchitectureSystem.Application.Exceptions;
using CleanArchitectureSystem.Application.Models.Identity;
using CleanArchitectureSystem.Application.Response;
using CleanArchitectureSystem.Identity.EntityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArchitectureSystem.Identity.Services
{
    public class AppAuthService(UserManager<AppUser> userManager, IOptions<JwtSettings> jwtSettings, SignInManager<AppUser> signInManager,
        ILogger<AppAuthService> logger) : IAppAuthServiceRepository
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

            if (!user.IsActive)
            {
                //throw new BadRequestException($"User with {request.Email} is not active.");
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = $"User with Email: {request.Email} is not active."
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
                UserName = user.UserName,
                IsSuccess = true,
                Message = "Login successful"
            };

            return response;
        }

        public async Task<IdentityResultResponse> Register(RegistrationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate data              
                var validator = new AppAuthServiceValidatorRegistration(request);
                var validatorResult = await validator.ValidateAsync(request, cancellationToken);

                if (validatorResult.Errors.Count != 0)
                {
                    _logger.LogWarning("Validation failed when creating the account: {Errors}", validatorResult.Errors);
                    throw new BadRequestException("Validation failed", validatorResult);
                }

                var existingEmail = await _userManager.FindByEmailAsync(request.Email);
                if (existingEmail != null)
                {
                    var failure = new FluentValidation.Results.ValidationResult(
                    [
                        new FluentValidation.Results.ValidationFailure(nameof(request.Email), "Email is already in use.")
                    ]);

                    throw new BadRequestException("Email already exists", failure);
                }

                var user = new AppUser
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    UserName = request.UserName,
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = null // Set to null initially
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                _logger.LogInformation("Created user: Username = {Username}, Id = {Id}", user.UserName, user.Id);

                if (!result.Succeeded)
                {
                    var identityFailures = result.Errors
                        .Select(e => new FluentValidation.Results.ValidationFailure("", e.Description))
                        .ToList();

                    var validationResult = new FluentValidation.Results.ValidationResult(identityFailures);

                    _logger.LogWarning("Identity validation failed for {Email}: {Errors}", request.Email, identityFailures);

                    throw new BadRequestException("Identity validation failed", validationResult);
                }

                user = await _userManager.FindByEmailAsync(user.Email); // Refresh the tracked user

                var roleResult = await _userManager.AddToRoleAsync(user, "User");

                if (!roleResult.Succeeded)
                {
                    var errorList = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    _logger.LogError("Failed to assign role to {Email}: {Errors}", user.Email, errorList);
                    throw new Exception($"Could not assign role: {errorList}");
                }


                return new IdentityResultResponse
                {
                    IsSuccess = true,
                    Message = "User registered successfully",
                    Email = user.Email
                };
            }
            catch (BadRequestException ex)
            {
                _logger.LogError(ex, "Validation error occurred for Username: {UserName}", request.UserName);

                // Return structured validation errors
                return new IdentityResultResponse
                {
                    IsSuccess = false,
                    Message = "App Authentication Service Validation failed.",
                    ValidationErrors = ex.ValidationErrors,
                    Id = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user");

                throw new ApplicationException("An unexpected error occurred while registering the user. Please try again.");
            }

        }

        public async Task<CustomResultResponse> UpdateUserAccount(string username, string email, [FromBody] UpdateRequest request)
        {
            try
            {
                // Pre-check inputs
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email))
                {
                    return Fail("Username and Email must be provided.");
                }

                // Retrieve and confirm user
                var user = await GetVerifiedUserAsync(username, email);
                if (user == null)
                {
                    return Fail("The provided username and email do not match the same account.");
                }

                // Validate payload
                var validator = new AppAuthServiceValidatorUpdateAccount();
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    LogValidation("Updating the account", validationResult.Errors);
                    throw new BadRequestException("Update validation failed", validationResult);
                }

                // Password update if require
                if (!string.IsNullOrWhiteSpace(request.Password))
                {
                    await UpdateUserPasswordAsync(user, request.Password);
                }

                // Role update if required
                if (!string.IsNullOrWhiteSpace(request.UserRole))
                {
                    await UpdateUserRoleAsync(user, request.UserRole);
                }

                // Update fields
                user.UserName = request.Username ?? user.UserName;
                user.IsActive = request.IsActive;
                user.ModifiedDate = DateTime.UtcNow;

                // Persist changes
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    HandleFailure("User update failed", updateResult.Errors, user.Id);
                }

                _logger.LogInformation("User account updated: {UserId}", user.Id);

                return new CustomResultResponse
                {
                    IsSuccess = true,
                    Message = "User account updated successfully.",
                    Id = user.Id
                };
            }
            catch (BadRequestException ex)
            {
                _logger.LogError(ex, "Validation error while updating user: {Username}", request.Username);
                return new CustomResultResponse
                {
                    IsSuccess = false,
                    Message = "User account update validation failed.",
                    ValidationErrors = ex.ValidationErrors,
                    Id = request.Username
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during account update: {Username}", request.Username);
                throw new ApplicationException("An unexpected error occurred while updating the user.");
            }
        }

        // Additional methods (can be added here as needed)
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
                new Claim("firstName", user.FirstName ?? ""),
                new Claim("isActive", user.IsActive.ToString()),

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
               expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes - 1),
               signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
        private async Task<AppUser?> GetVerifiedUserAsync(string username, string email)
        {
            var userByName = await _userManager.FindByNameAsync(username);
            var userByEmail = await _userManager.FindByEmailAsync(email);

            if (userByName == null || userByEmail == null) return null;
            return userByName.Id == userByEmail.Id ? userByName : null;
        }

        private async Task UpdateUserPasswordAsync(AppUser user, string newPassword)
        {
            var removeResult = await _userManager.RemovePasswordAsync(user);
            if (!removeResult.Succeeded)
            {
                HandleFailure("Password removal failed", removeResult.Errors, user.Id);
            }

            var addResult = await _userManager.AddPasswordAsync(user, newPassword);
            if (!addResult.Succeeded)
            {
                HandleFailure("Password update failed", addResult.Errors, user.Id);
            }

            _logger.LogInformation("Password updated successfully for User ID: {UserId}", user.Id);
        }

        private async Task UpdateUserRoleAsync(AppUser user, string newRole)
        {
            if (string.IsNullOrWhiteSpace(newRole))
            {
                _logger.LogWarning("No role provided for update. Skipping role assignment.");
                return;
            }

            // Remove existing roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    HandleFailure("Failed to remove current roles", removeResult.Errors, user.Id);
                }
            }

            // Assign new role
            var addResult = await _userManager.AddToRoleAsync(user, newRole);
            if (!addResult.Succeeded)
            {
                HandleFailure($"Failed to assign role '{newRole}'", addResult.Errors, user.Id);
            }

            _logger.LogInformation("Role '{Role}' assigned to User ID: {UserId}", newRole, user.Id);
        }

        private void HandleFailure(string message, IEnumerable<IdentityError> errors, string userId)
        {
            var failures = errors
                .Select(e => new FluentValidation.Results.ValidationFailure("", e.Description))
                .ToList();

            var failureResult = new FluentValidation.Results.ValidationResult(failures);
            _logger.LogError("{Message} for ID {UserId}: {Errors}", message, userId, failures);
            throw new BadRequestException(message, failureResult);
        }

        private void LogValidation(string context, IEnumerable<FluentValidation.Results.ValidationFailure> errors)
        {
            _logger.LogWarning("Validation failed during {Context}: {Errors}", context, errors);
        }

        private CustomResultResponse Fail(string message)
        {
            return new CustomResultResponse { IsSuccess = false, Message = message };
        }
    }
}
