using MediatR;
using Shared.Contracts.Models;

namespace Auth.Application.Features.Auth.Commands;

public class SetupTwoFactorCommand : IRequest<TwoFactorSetupResponse>
{
    public string UserId { get; set; } = string.Empty;
}

public class SetupTwoFactorCommandHandler : IRequestHandler<SetupTwoFactorCommand, TwoFactorSetupResponse>
{
    private readonly Auth.Application.Interfaces.IAuthService _authService;

    public SetupTwoFactorCommandHandler(Auth.Application.Interfaces.IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<TwoFactorSetupResponse> Handle(SetupTwoFactorCommand request, CancellationToken cancellationToken)
    {
        return await _authService.SetupTwoFactorAsync(request.UserId);
    }
}

public class EnableTwoFactorCommand : IRequest<AuthResult>
{
    public string UserId { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}

public class EnableTwoFactorCommandHandler : IRequestHandler<EnableTwoFactorCommand, AuthResult>
{
    private readonly Auth.Application.Interfaces.IAuthService _authService;

    public EnableTwoFactorCommandHandler(Auth.Application.Interfaces.IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResult> Handle(EnableTwoFactorCommand request, CancellationToken cancellationToken)
    {
        return await _authService.EnableTwoFactorAsync(request.UserId, request.Secret, request.Code);
    }
}

public class DisableTwoFactorCommand : IRequest<AuthResult>
{
    public string UserId { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}

public class DisableTwoFactorCommandHandler : IRequestHandler<DisableTwoFactorCommand, AuthResult>
{
    private readonly Auth.Application.Interfaces.IAuthService _authService;

    public DisableTwoFactorCommandHandler(Auth.Application.Interfaces.IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResult> Handle(DisableTwoFactorCommand request, CancellationToken cancellationToken)
    {
        return await _authService.DisableTwoFactorAsync(request.UserId, request.Code);
    }
} 