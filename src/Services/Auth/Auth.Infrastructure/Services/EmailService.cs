using Auth.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Auth.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        await SendEmailAsync(to, subject, body, true);
    }

    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
    {
        // In development, just log the email
        // In production, integrate with a real email service like SendGrid, MailKit, etc.
        _logger.LogInformation("Email sent to {To}: {Subject}", to, subject);
        _logger.LogDebug("Email body: {Body}", body);

        // Simulate email sending delay
        await Task.Delay(100);
    }
} 