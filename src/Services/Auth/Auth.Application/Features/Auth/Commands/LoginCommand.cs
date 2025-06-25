using MediatR;
using Shared.Contracts.Models;

namespace Auth.Application.Features.Auth.Commands;

public class LoginCommand : IRequest<LoginResponse>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
} 