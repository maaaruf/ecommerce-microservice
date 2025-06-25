using Auth.Domain.Entities;
using Auth.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Shared.Contracts.Models;
using Shared.Contracts.Interfaces;
using Auth.Application.Interfaces;

namespace Auth.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly Shared.Contracts.Interfaces.IJwtService _jwtService;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public AuthService(
        IUserRepository userRepository,
        Shared.Contracts.Interfaces.IJwtService jwtService,
        IConfiguration configuration,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<UserDto> RegisterUserAsync(CreateUserRequest request)
    {
        // Check if user already exists
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }

        // Create new user
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = request.Email,
            Username = request.Username,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Hash password (in real implementation, use proper password hashing)
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        await _userRepository.AddAsync(user);

        return _mapper.Map<UserDto>(user);
    }

    public async Task<LoginResponse> LoginAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            throw new InvalidOperationException("Invalid username or password");
        }

        if (!user.IsActive)
        {
            throw new InvalidOperationException("User account is deactivated");
        }

        // Update last login
        user.LastLoginAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        var userDto = _mapper.Map<UserDto>(user);
        var accessToken = _jwtService.GenerateAccessToken(userDto);
        var refreshToken = _jwtService.GenerateRefreshToken();

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            User = userDto
        };
    }

    public async Task<LoginResponse> RefreshTokenAsync(string refreshToken)
    {
        // In a real implementation, validate the refresh token against stored tokens
        // For now, we'll generate a new token
        var userId = _jwtService.GetUserIdFromToken(refreshToken);
        if (string.IsNullOrEmpty(userId))
        {
            throw new InvalidOperationException("Invalid refresh token");
        }

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null || !user.IsActive)
        {
            throw new InvalidOperationException("User not found or inactive");
        }

        var userDto = _mapper.Map<UserDto>(user);
        var accessToken = _jwtService.GenerateAccessToken(userDto);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            User = userDto
        };
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

    public async Task LogoutAsync(string userId)
    {
        // In a real implementation, invalidate the refresh token
        // For now, we'll just log the logout
        var user = await _userRepository.GetByIdAsync(userId);
        if (user != null)
        {
            // Update last activity or log logout event
            await _userRepository.UpdateAsync(user);
        }
    }
} 