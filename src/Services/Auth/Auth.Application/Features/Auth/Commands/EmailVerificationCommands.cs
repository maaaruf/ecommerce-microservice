using MediatR;
using Shared.Contracts.Models;

namespace Auth.Application.Features.Auth.Commands;

public class VerifyEmailCommand : IRequest<AuthResult>
{
    public string Token { get; set; } = string.Empty;
}

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, AuthResult>
{
    private readonly Auth.Application.Interfaces.IAuthService _authService;

    public VerifyEmailCommandHandler(Auth.Application.Interfaces.IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResult> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        return await _authService.VerifyEmailAsync(request.Token);
    }
}

public class ResendEmailVerificationCommand : IRequest<AuthResult>
{
    public string Email { get; set; } = string.Empty;
}

public class ResendEmailVerificationCommandHandler : IRequestHandler<ResendEmailVerificationCommand, AuthResult>
{
    private readonly Auth.Application.Interfaces.IAuthService _authService;

    public ResendEmailVerificationCommandHandler(Auth.Application.Interfaces.IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResult> Handle(ResendEmailVerificationCommand request, CancellationToken cancellationToken)
    {
        return await _authService.ResendEmailVerificationAsync(request.Email);
    }
} 