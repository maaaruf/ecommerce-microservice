using MediatR;
using Shared.Contracts.Models;

namespace Auth.Application.Features.Auth.Commands;

public class LogoutCommand : IRequest<AuthResult>
{
    public string RefreshToken { get; set; } = string.Empty;
}

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, AuthResult>
{
    private readonly Auth.Application.Interfaces.IAuthService _authService;

    public LogoutCommandHandler(Auth.Application.Interfaces.IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResult> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        return await _authService.LogoutAsync(request.RefreshToken);
    }
} 