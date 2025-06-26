using Auth.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Auth.Infrastructure.Services;

public class KeycloakService : IKeycloakService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<KeycloakService> _logger;
    private readonly string _authority;
    private readonly string _clientId;
    private readonly string _clientSecret;

    public KeycloakService(HttpClient httpClient, IConfiguration configuration, ILogger<KeycloakService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
        
        _authority = _configuration["Keycloak:Authority"] ?? throw new InvalidOperationException("Keycloak Authority not configured");
        _clientId = _configuration["Keycloak:ClientId"] ?? throw new InvalidOperationException("Keycloak ClientId not configured");
        _clientSecret = _configuration["Keycloak:ClientSecret"] ?? throw new InvalidOperationException("Keycloak ClientSecret not configured");
    }

    public async Task<string> GetAuthorizationUrlAsync(string redirectUri, string state)
    {
        var scope = "openid profile email";
        var responseType = "code";
        
        var queryParams = new Dictionary<string, string>
        {
            ["client_id"] = _clientId,
            ["response_type"] = responseType,
            ["scope"] = scope,
            ["redirect_uri"] = redirectUri,
            ["state"] = state
        };

        var queryString = string.Join("&", queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
        return $"{_authority}/protocol/openid-connect/auth?{queryString}";
    }

    public async Task<TokenResponse> ExchangeCodeForTokenAsync(string code, string redirectUri)
    {
        var tokenEndpoint = $"{_authority}/protocol/openid-connect/token";
        
        var parameters = new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["client_id"] = _clientId,
            ["client_secret"] = _clientSecret,
            ["code"] = code,
            ["redirect_uri"] = redirectUri
        };

        var content = new FormUrlEncodedContent(parameters);
        
        try
        {
            var response = await _httpClient.PostAsync(tokenEndpoint, content);
            response.EnsureSuccessStatusCode();
            
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var tokenData = JsonSerializer.Deserialize<JsonElement>(jsonResponse);
            
            return new TokenResponse
            {
                AccessToken = tokenData.GetProperty("access_token").GetString() ?? string.Empty,
                RefreshToken = tokenData.GetProperty("refresh_token").GetString() ?? string.Empty,
                IdToken = tokenData.GetProperty("id_token").GetString() ?? string.Empty,
                TokenType = tokenData.GetProperty("token_type").GetString() ?? string.Empty,
                ExpiresIn = tokenData.GetProperty("expires_in").GetInt32(),
                Scope = tokenData.GetProperty("scope").GetString() ?? string.Empty
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to exchange code for token");
            throw new InvalidOperationException("Failed to exchange authorization code for token", ex);
        }
    }

    public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
    {
        var tokenEndpoint = $"{_authority}/protocol/openid-connect/token";
        
        var parameters = new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["client_id"] = _clientId,
            ["client_secret"] = _clientSecret,
            ["refresh_token"] = refreshToken
        };

        var content = new FormUrlEncodedContent(parameters);
        
        try
        {
            var response = await _httpClient.PostAsync(tokenEndpoint, content);
            response.EnsureSuccessStatusCode();
            
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var tokenData = JsonSerializer.Deserialize<JsonElement>(jsonResponse);
            
            return new TokenResponse
            {
                AccessToken = tokenData.GetProperty("access_token").GetString() ?? string.Empty,
                RefreshToken = tokenData.GetProperty("refresh_token").GetString() ?? string.Empty,
                IdToken = tokenData.GetProperty("id_token").GetString() ?? string.Empty,
                TokenType = tokenData.GetProperty("token_type").GetString() ?? string.Empty,
                ExpiresIn = tokenData.GetProperty("expires_in").GetInt32(),
                Scope = tokenData.GetProperty("scope").GetString() ?? string.Empty
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to refresh token");
            throw new InvalidOperationException("Failed to refresh token", ex);
        }
    }

    public async Task<UserInfo> GetUserInfoAsync(string accessToken)
    {
        var userInfoEndpoint = $"{_authority}/protocol/openid-connect/userinfo";
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
        try
        {
            var response = await _httpClient.GetAsync(userInfoEndpoint);
            response.EnsureSuccessStatusCode();
            
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var userData = JsonSerializer.Deserialize<JsonElement>(jsonResponse);
            
            var userInfo = new UserInfo
            {
                Sub = userData.GetProperty("sub").GetString() ?? string.Empty,
                Email = userData.GetProperty("email").GetString() ?? string.Empty,
                Username = userData.GetProperty("preferred_username").GetString() ?? string.Empty,
                GivenName = userData.GetProperty("given_name").GetString() ?? string.Empty,
                FamilyName = userData.GetProperty("family_name").GetString() ?? string.Empty,
                Name = userData.GetProperty("name").GetString() ?? string.Empty,
                EmailVerified = userData.GetProperty("email_verified").GetBoolean()
            };

            // Extract roles from realm_access if available
            if (userData.TryGetProperty("realm_access", out var realmAccess) && 
                realmAccess.TryGetProperty("roles", out var roles))
            {
                userInfo.Roles = roles.EnumerateArray()
                    .Select(r => r.GetString() ?? string.Empty)
                    .Where(r => !string.IsNullOrEmpty(r))
                    .ToList();
            }

            // Extract additional claims
            foreach (var property in userData.EnumerateObject())
            {
                if (!userInfo.GetType().GetProperties().Any(p => p.Name.Equals(property.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    userInfo.Claims[property.Name] = property.Value.GetRawText();
                }
            }

            return userInfo;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user info");
            throw new InvalidOperationException("Failed to get user info", ex);
        }
    }

    public async Task<bool> ValidateTokenAsync(string accessToken)
    {
        var introspectionEndpoint = $"{_authority}/protocol/openid-connect/token/introspect";
        
        var parameters = new Dictionary<string, string>
        {
            ["token"] = accessToken,
            ["client_id"] = _clientId,
            ["client_secret"] = _clientSecret
        };

        var content = new FormUrlEncodedContent(parameters);
        
        try
        {
            var response = await _httpClient.PostAsync(introspectionEndpoint, content);
            response.EnsureSuccessStatusCode();
            
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var introspectionData = JsonSerializer.Deserialize<JsonElement>(jsonResponse);
            
            return introspectionData.GetProperty("active").GetBoolean();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to validate token");
            return false;
        }
    }

    public async Task<bool> RevokeTokenAsync(string token, string tokenTypeHint = "refresh_token")
    {
        var revocationEndpoint = $"{_authority}/protocol/openid-connect/logout";
        
        var parameters = new Dictionary<string, string>
        {
            ["client_id"] = _clientId,
            ["client_secret"] = _clientSecret,
            ["refresh_token"] = token
        };

        var content = new FormUrlEncodedContent(parameters);
        
        try
        {
            var response = await _httpClient.PostAsync(revocationEndpoint, content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to revoke token");
            return false;
        }
    }

    public async Task<string> GetLogoutUrlAsync(string redirectUri, string idToken)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["client_id"] = _clientId,
            ["id_token_hint"] = idToken,
            ["post_logout_redirect_uri"] = redirectUri
        };

        var queryString = string.Join("&", queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
        return $"{_authority}/protocol/openid-connect/logout?{queryString}";
    }
} 