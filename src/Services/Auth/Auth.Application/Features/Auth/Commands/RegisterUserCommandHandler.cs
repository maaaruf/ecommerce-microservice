using Auth.Application.Interfaces;
using MediatR;
using Shared.Contracts.Models;

namespace Auth.Application.Features.Auth.Commands;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly IAuthService _authService;

    public RegisterUserCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var createUserRequest = new CreateUserRequest
        {
            Email = request.Email,
            Username = request.Username,
            Password = request.Password,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber
        };

        return await _authService.RegisterUserAsync(createUserRequest);
    }
} 