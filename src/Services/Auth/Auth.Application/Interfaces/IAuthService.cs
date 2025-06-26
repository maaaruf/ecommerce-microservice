using Shared.Contracts.Models;
using Auth.Application.Interfaces;

namespace Auth.Application.Interfaces;

public interface IAuthService
{
    // Keycloak OAuth2/OIDC methods
    Task<string> GetAuthorizationUrlAsync(string redirectUri, string state);
    Task<LoginResponse> HandleCallbackAsync(string code, string state, string redirectUri);
    Task<LoginResponse> RefreshTokenAsync(string refreshToken);
    Task<bool> ValidateTokenAsync(string accessToken);
    Task<UserInfo> GetUserInfoAsync(string accessToken);
    Task<AuthResult> LogoutAsync(string refreshToken);
    Task<string> GetLogoutUrlAsync(string redirectUri, string idToken);
    
    // User management methods
    Task<UserDto> GetUserByIdAsync(string userId);
    Task<UserDto> UpdateProfileAsync(string userId, UpdateProfileRequest request);
    
    // Legacy methods for backward compatibility (will be removed in future versions)
    Task<UserDto> RegisterUserAsync(CreateUserRequest request);
    Task<LoginResponse> LoginAsync(string username, string password, string? twoFactorCode = null);
    Task<AuthResult> ForgotPasswordAsync(string email);
    Task<AuthResult> ResetPasswordAsync(string token, string newPassword);
    Task<AuthResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    Task<AuthResult> VerifyEmailAsync(string token);
    Task<AuthResult> ResendEmailVerificationAsync(string email);
    Task<TwoFactorSetupResponse> SetupTwoFactorAsync(string userId);
    Task<AuthResult> EnableTwoFactorAsync(string userId, string secret, string code);
    Task<AuthResult> DisableTwoFactorAsync(string userId, string code);
} 