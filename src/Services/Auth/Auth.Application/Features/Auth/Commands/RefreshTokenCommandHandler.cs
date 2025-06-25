using Auth.Application.Interfaces;
using MediatR;
using Shared.Contracts.Models;

namespace Auth.Application.Features.Auth.Commands;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponse>
{
    private readonly IAuthService _authService;

    public RefreshTokenCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<LoginResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await _authService.RefreshTokenAsync(request.RefreshToken);
    }
} 