using MediatR;
using Shared.Contracts.Models;
using Auth.Application.Interfaces;

namespace Auth.Application.Features.Auth.Commands;

public class GetAuthorizationUrlCommand : IRequest<string>
{
    public string RedirectUri { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
}

public class GetAuthorizationUrlCommandHandler : IRequestHandler<GetAuthorizationUrlCommand, string>
{
    private readonly Auth.Application.Interfaces.IAuthService _authService;

    public GetAuthorizationUrlCommandHandler(Auth.Application.Interfaces.IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<string> Handle(GetAuthorizationUrlCommand request, CancellationToken cancellationToken)
    {
        return await _authService.GetAuthorizationUrlAsync(request.RedirectUri, request.State);
    }
}

public class HandleCallbackCommand : IRequest<LoginResponse>
{
    public string Code { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string RedirectUri { get; set; } = string.Empty;
}

public class HandleCallbackCommandHandler : IRequestHandler<HandleCallbackCommand, LoginResponse>
{
    private readonly Auth.Application.Interfaces.IAuthService _authService;

    public HandleCallbackCommandHandler(Auth.Application.Interfaces.IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<LoginResponse> Handle(HandleCallbackCommand request, CancellationToken cancellationToken)
    {
        return await _authService.HandleCallbackAsync(request.Code, request.State, request.RedirectUri);
    }
}

public class ValidateTokenCommand : IRequest<bool>
{
    public string AccessToken { get; set; } = string.Empty;
}

public class ValidateTokenCommandHandler : IRequestHandler<ValidateTokenCommand, bool>
{
    private readonly Auth.Application.Interfaces.IAuthService _authService;

    public ValidateTokenCommandHandler(Auth.Application.Interfaces.IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<bool> Handle(ValidateTokenCommand request, CancellationToken cancellationToken)
    {
        return await _authService.ValidateTokenAsync(request.AccessToken);
    }
}

public class GetUserInfoCommand : IRequest<UserInfo>
{
    public string AccessToken { get; set; } = string.Empty;
}

public class GetUserInfoCommandHandler : IRequestHandler<GetUserInfoCommand, UserInfo>
{
    private readonly Auth.Application.Interfaces.IAuthService _authService;

    public GetUserInfoCommandHandler(Auth.Application.Interfaces.IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<UserInfo> Handle(GetUserInfoCommand request, CancellationToken cancellationToken)
    {
        return await _authService.GetUserInfoAsync(request.AccessToken);
    }
}

public class GetLogoutUrlCommand : IRequest<string>
{
    public string RedirectUri { get; set; } = string.Empty;
    public string IdToken { get; set; } = string.Empty;
}

public class GetLogoutUrlCommandHandler : IRequestHandler<GetLogoutUrlCommand, string>
{
    private readonly Auth.Application.Interfaces.IAuthService _authService;

    public GetLogoutUrlCommandHandler(Auth.Application.Interfaces.IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<string> Handle(GetLogoutUrlCommand request, CancellationToken cancellationToken)
    {
        return await _authService.GetLogoutUrlAsync(request.RedirectUri, request.IdToken);
    }
} 