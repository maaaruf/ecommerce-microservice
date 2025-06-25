using MediatR;

namespace Auth.Application.Features.Auth.Commands;

public class LogoutCommand : IRequest<Unit>
{
    public string UserId { get; set; } = string.Empty;
} 