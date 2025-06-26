using MediatR;
using Shared.Contracts.Models;

namespace Auth.Application.Features.Auth.Commands;

public class ResetPasswordCommand : IRequest<AuthResult>
{
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, AuthResult>
{
    private readonly Auth.Application.Interfaces.IAuthService _authService;

    public ResetPasswordCommandHandler(Auth.Application.Interfaces.IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        return await _authService.ResetPasswordAsync(request.Token, request.NewPassword);
    }
} 