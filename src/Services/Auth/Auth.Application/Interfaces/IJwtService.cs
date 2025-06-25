using Shared.Contracts.Models;

namespace Auth.Application.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(UserDto user);
    string GenerateRefreshToken();
    bool ValidateToken(string token);
    string? GetUserIdFromToken(string token);
} 