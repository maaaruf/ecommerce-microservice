using Shared.Contracts.Models;

namespace Auth.Application.Interfaces;

public interface IKeycloakService
{
    Task<string> GetAuthorizationUrlAsync(string redirectUri, string state);
    Task<TokenResponse> ExchangeCodeForTokenAsync(string code, string redirectUri);
    Task<TokenResponse> RefreshTokenAsync(string refreshToken);
    Task<UserInfo> GetUserInfoAsync(string accessToken);
    Task<bool> ValidateTokenAsync(string accessToken);
    Task<bool> RevokeTokenAsync(string token, string tokenTypeHint = "refresh_token");
    Task<string> GetLogoutUrlAsync(string redirectUri, string idToken);
}

public class TokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string IdToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
    public string Scope { get; set; } = string.Empty;
}

public class UserInfo
{
    public string Sub { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string GivenName { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool EmailVerified { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
    public Dictionary<string, object> Claims { get; set; } = new Dictionary<string, object>();
} 