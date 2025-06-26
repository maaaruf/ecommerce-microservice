using MediatR;
using Shared.Contracts.Models;

namespace Auth.Application.Features.Auth.Commands;

public class ChangePasswordCommand : IRequest<AuthResult>
{
    public string UserId { get; set; } = string.Empty;
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, AuthResult>
{
    private readonly Auth.Application.Interfaces.IAuthService _authService;

    public ChangePasswordCommandHandler(Auth.Application.Interfaces.IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        return await _authService.ChangePasswordAsync(request.UserId, request.CurrentPassword, request.NewPassword);
    }
} 