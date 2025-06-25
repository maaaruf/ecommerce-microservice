using Shared.Contracts.Models;

namespace Auth.Application.Interfaces;

public interface IAuthService
{
    Task<UserDto> RegisterUserAsync(CreateUserRequest request);
    Task<LoginResponse> LoginAsync(string username, string password);
    Task<LoginResponse> RefreshTokenAsync(string refreshToken);
    Task<UserDto> GetUserByIdAsync(string userId);
    Task LogoutAsync(string userId);
} 