using Auth.Application.Features.Auth.Commands;
using Auth.Application.Features.Auth.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Models;

namespace Auth.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("login")]
    public async Task<ActionResult<string>> GetLoginUrl([FromQuery] string redirectUri, [FromQuery] string state)
    {
        var command = new GetAuthorizationUrlCommand
        {
            RedirectUri = redirectUri,
            State = state
        };

        var result = await _mediator.Send(command);
        return Ok(new { loginUrl = result });
    }

    [HttpGet("callback")]
    public async Task<ActionResult<LoginResponse>> HandleCallback(
        [FromQuery] string code, 
        [FromQuery] string state, 
        [FromQuery] string redirectUri)
    {
        var command = new HandleCallbackCommand
        {
            Code = code,
            State = state,
            RedirectUri = redirectUri
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<LoginResponse>> RefreshToken([FromBody] string refreshToken)
    {
        var command = new RefreshTokenCommand
        {
            RefreshToken = refreshToken
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("validate")]
    public async Task<ActionResult<bool>> ValidateToken([FromBody] string accessToken)
    {
        var command = new ValidateTokenCommand
        {
            AccessToken = accessToken
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("userinfo")]
    public async Task<ActionResult<UserInfo>> GetUserInfo([FromBody] string accessToken)
    {
        var command = new GetUserInfoCommand
        {
            AccessToken = accessToken
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<UserDto>> GetProfile()
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var query = new GetUserByIdQuery { UserId = userId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<ActionResult<UserDto>> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var command = new UpdateProfileCommand
        {
            UserId = userId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            ProfilePictureUrl = request.ProfilePictureUrl,
            TimeZone = request.TimeZone,
            Language = request.Language
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("logout")]
    public async Task<ActionResult<AuthResult>> Logout([FromBody] string refreshToken)
    {
        var command = new LogoutCommand { RefreshToken = refreshToken };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("logout-url")]
    public async Task<ActionResult<string>> GetLogoutUrl([FromQuery] string redirectUri, [FromQuery] string idToken)
    {
        var command = new GetLogoutUrlCommand
        {
            RedirectUri = redirectUri,
            IdToken = idToken
        };

        var result = await _mediator.Send(command);
        return Ok(new { logoutUrl = result });
    }

    // Legacy endpoints for backward compatibility - these will be removed in future versions
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] CreateUserRequest request)
    {
        return BadRequest(new { message = "User registration is now handled through Keycloak. Please use OAuth2/OIDC flow." });
    }

    [HttpPost("login-legacy")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        return BadRequest(new { message = "Direct login is not supported. Please use OAuth2/OIDC flow with Keycloak." });
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult<AuthResult>> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        return BadRequest(new { message = "Password reset is now handled through Keycloak." });
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult<AuthResult>> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        return BadRequest(new { message = "Password reset is now handled through Keycloak." });
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<ActionResult<AuthResult>> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        return BadRequest(new { message = "Password change is now handled through Keycloak." });
    }

    [HttpPost("verify-email")]
    public async Task<ActionResult<AuthResult>> VerifyEmail([FromBody] VerifyEmailRequest request)
    {
        return BadRequest(new { message = "Email verification is now handled through Keycloak." });
    }

    [HttpPost("resend-email-verification")]
    public async Task<ActionResult<AuthResult>> ResendEmailVerification([FromBody] ResendEmailVerificationRequest request)
    {
        return BadRequest(new { message = "Email verification is now handled through Keycloak." });
    }

    [Authorize]
    [HttpGet("two-factor/setup")]
    public async Task<ActionResult<TwoFactorSetupResponse>> SetupTwoFactor()
    {
        return BadRequest(new { message = "Two-factor authentication is now handled through Keycloak." });
    }

    [Authorize]
    [HttpPost("two-factor/enable")]
    public async Task<ActionResult<AuthResult>> EnableTwoFactor([FromBody] EnableTwoFactorRequest request)
    {
        return BadRequest(new { message = "Two-factor authentication is now handled through Keycloak." });
    }

    [Authorize]
    [HttpPost("two-factor/disable")]
    public async Task<ActionResult<AuthResult>> DisableTwoFactor([FromBody] DisableTwoFactorRequest request)
    {
        return BadRequest(new { message = "Two-factor authentication is now handled through Keycloak." });
    }
} 