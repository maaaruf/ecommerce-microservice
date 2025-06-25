using MediatR;
using Shared.Contracts.Models;

namespace Auth.Application.Features.Auth.Queries;

public class GetUserByIdQuery : IRequest<UserDto>
{
    public string UserId { get; set; } = string.Empty;
} 