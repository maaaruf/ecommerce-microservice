using Auth.Domain.Entities;
using Auth.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Shared.Contracts.Models;
using Auth.Application.Interfaces;

namespace Auth.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IKeycloakService _keycloakService;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;

    public AuthService(
        IUserRepository userRepository,
        IKeycloakService keycloakService,
        IConfiguration configuration,
        IMapper mapper,
        IEmailService emailService)
    {
        _userRepository = userRepository;
        _keycloakService = keycloakService;
        _configuration = configuration;
        _mapper = mapper;
        _emailService = emailService;
    }

    public async Task<string> GetAuthorizationUrlAsync(string redirectUri, string state)
    {
        return await _keycloakService.GetAuthorizationUrlAsync(redirectUri, state);
    }

    public async Task<LoginResponse> HandleCallbackAsync(string code, string state, string redirectUri)
    {
        // Exchange authorization code for tokens
        var tokenResponse = await _keycloakService.ExchangeCodeForTokenAsync(code, redirectUri);
        
        // Get user information from Keycloak
        var userInfo = await _keycloakService.GetUserInfoAsync(tokenResponse.AccessToken);
        
        // Check if user exists in our database, if not create them
        var user = await _userRepository.GetByEmailAsync(userInfo.Email);
        if (user == null)
        {
            // Create new user from Keycloak user info
            user = new User
            {
                Id = userInfo.Sub,
                Email = userInfo.Email,
                Username = userInfo.Username,
                FirstName = userInfo.GivenName,
                LastName = userInfo.FamilyName,
                IsActive = true,
                IsEmailVerified = userInfo.EmailVerified,
                Roles = userInfo.Roles.Any() ? userInfo.Roles : new List<string> { "User" },
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
        }
        else
        {
            // Update existing user with latest info from Keycloak
            user.FirstName = userInfo.GivenName;
            user.LastName = userInfo.FamilyName;
            user.Username = userInfo.Username;
            user.IsEmailVerified = userInfo.EmailVerified;
            user.Roles = userInfo.Roles.Any() ? userInfo.Roles : user.Roles;
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedBy = userInfo.Sub;

            await _userRepository.UpdateAsync(user);
        }

        var userDto = _mapper.Map<UserDto>(user);

        return new LoginResponse
        {
            AccessToken = tokenResponse.AccessToken,
            RefreshToken = tokenResponse.RefreshToken,
            ExpiresAt = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn),
            User = userDto,
            RequirePasswordChange = user.RequirePasswordChange
        };
    }

    public async Task<LoginResponse> RefreshTokenAsync(string refreshToken)
    {
        var tokenResponse = await _keycloakService.RefreshTokenAsync(refreshToken);
        
        // Get updated user information
        var userInfo = await _keycloakService.GetUserInfoAsync(tokenResponse.AccessToken);
        var user = await _userRepository.GetByIdAsync(userInfo.Sub);
        
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        var userDto = _mapper.Map<UserDto>(user);

        return new LoginResponse
        {
            AccessToken = tokenResponse.AccessToken,
            RefreshToken = tokenResponse.RefreshToken,
            ExpiresAt = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn),
            User = userDto
        };
    }

    public async Task<bool> ValidateTokenAsync(string accessToken)
    {
        return await _keycloakService.ValidateTokenAsync(accessToken);
    }

    public async Task<UserInfo> GetUserInfoAsync(string accessToken)
    {
        return await _keycloakService.GetUserInfoAsync(accessToken);
    }

    public async Task<UserDto> GetUserByIdAsync(string userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateProfileAsync(string userId, UpdateProfileRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        user.ProfilePictureUrl = request.ProfilePictureUrl;
        user.TimeZone = request.TimeZone;
        user.Language = request.Language;
        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = userId;

        await _userRepository.UpdateAsync(user);

        return _mapper.Map<UserDto>(user);
    }

    public async Task<AuthResult> LogoutAsync(string refreshToken)
    {
        var success = await _keycloakService.RevokeTokenAsync(refreshToken);
        
        return new AuthResult
        {
            Success = success,
            Message = success ? "Logged out successfully" : "Failed to logout"
        };
    }

    public async Task<string> GetLogoutUrlAsync(string redirectUri, string idToken)
    {
        return await _keycloakService.GetLogoutUrlAsync(redirectUri, idToken);
    }

    // Legacy methods for backward compatibility - these will be removed in future versions
    public async Task<UserDto> RegisterUserAsync(CreateUserRequest request)
    {
        throw new NotImplementedException("User registration is now handled through Keycloak. Please use OAuth2/OIDC flow.");
    }

    public async Task<LoginResponse> LoginAsync(string username, string password, string? twoFactorCode = null)
    {
        throw new NotImplementedException("Direct login is not supported. Please use OAuth2/OIDC flow with Keycloak.");
    }

    public async Task<AuthResult> ForgotPasswordAsync(string email)
    {
        throw new NotImplementedException("Password reset is now handled through Keycloak.");
    }

    public async Task<AuthResult> ResetPasswordAsync(string token, string newPassword)
    {
        throw new NotImplementedException("Password reset is now handled through Keycloak.");
    }

    public async Task<AuthResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        throw new NotImplementedException("Password change is now handled through Keycloak.");
    }

    public async Task<AuthResult> VerifyEmailAsync(string token)
    {
        throw new NotImplementedException("Email verification is now handled through Keycloak.");
    }

    public async Task<AuthResult> ResendEmailVerificationAsync(string email)
    {
        throw new NotImplementedException("Email verification is now handled through Keycloak.");
    }

    public async Task<TwoFactorSetupResponse> SetupTwoFactorAsync(string userId)
    {
        throw new NotImplementedException("Two-factor authentication is now handled through Keycloak.");
    }

    public async Task<AuthResult> EnableTwoFactorAsync(string userId, string secret, string code)
    {
        throw new NotImplementedException("Two-factor authentication is now handled through Keycloak.");
    }

    public async Task<AuthResult> DisableTwoFactorAsync(string userId, string code)
    {
        throw new NotImplementedException("Two-factor authentication is now handled through Keycloak.");
    }
} 