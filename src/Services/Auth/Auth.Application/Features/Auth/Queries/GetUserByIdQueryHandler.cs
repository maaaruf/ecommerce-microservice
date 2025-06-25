using Auth.Application.Interfaces;
using MediatR;
using Shared.Contracts.Models;

namespace Auth.Application.Features.Auth.Queries;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IAuthService _authService;

    public GetUserByIdQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await _authService.GetUserByIdAsync(request.UserId);
    }
} 