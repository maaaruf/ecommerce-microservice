using MediatR;
using Shared.Contracts.Models;

namespace Auth.Application.Features.Auth.Commands;

public class RefreshTokenCommand : IRequest<LoginResponse>
{
    public string RefreshToken { get; set; } = string.Empty;
} 