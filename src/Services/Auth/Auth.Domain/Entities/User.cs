using System.ComponentModel.DataAnnotations;

namespace Auth.Domain.Entities;

public class User
{
    public string Id { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = true;
    public bool IsEmailVerified { get; set; } = false;
    public bool IsPhoneVerified { get; set; } = false;
    public bool IsTwoFactorEnabled { get; set; } = false;
    public string? TwoFactorSecret { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public DateTime? LastPasswordChangeAt { get; set; }
    public DateTime? EmailVerificationSentAt { get; set; }
    public DateTime? PasswordResetSentAt { get; set; }
    
    // Security fields
    public int FailedLoginAttempts { get; set; } = 0;
    public DateTime? LockoutEnd { get; set; }
    public string? EmailVerificationToken { get; set; }
    public string? PasswordResetToken { get; set; }
    public DateTime? PasswordResetTokenExpiresAt { get; set; }
    
    // Roles and permissions
    public List<string> Roles { get; set; } = new List<string>();
    public List<string> Permissions { get; set; } = new List<string>();
    
    // Account settings
    public bool RequirePasswordChange { get; set; } = false;
    public string? ProfilePictureUrl { get; set; }
    public string? TimeZone { get; set; } = "UTC";
    public string? Language { get; set; } = "en";
    
    // Audit fields
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Methods
    public bool IsLockedOut => LockoutEnd.HasValue && LockoutEnd.Value > DateTime.UtcNow;
    
    public void IncrementFailedLoginAttempts()
    {
        FailedLoginAttempts++;
        if (FailedLoginAttempts >= 5) // Lock after 5 failed attempts
        {
            LockoutEnd = DateTime.UtcNow.AddMinutes(15); // Lock for 15 minutes
        }
    }
    
    public void ResetFailedLoginAttempts()
    {
        FailedLoginAttempts = 0;
        LockoutEnd = null;
    }
    
    public void UpdateLastLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        ResetFailedLoginAttempts();
    }
    
    public void SetPasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
        LastPasswordChangeAt = DateTime.UtcNow;
        RequirePasswordChange = false;
    }
    
    public void GenerateEmailVerificationToken()
    {
        EmailVerificationToken = Guid.NewGuid().ToString();
        EmailVerificationSentAt = DateTime.UtcNow;
    }
    
    public void GeneratePasswordResetToken()
    {
        PasswordResetToken = Guid.NewGuid().ToString();
        PasswordResetTokenExpiresAt = DateTime.UtcNow.AddHours(24); // 24 hour expiry
        PasswordResetSentAt = DateTime.UtcNow;
    }
    
    public bool IsPasswordResetTokenValid()
    {
        return !string.IsNullOrEmpty(PasswordResetToken) && 
               PasswordResetTokenExpiresAt.HasValue && 
               PasswordResetTokenExpiresAt.Value > DateTime.UtcNow;
    }
    
    public void ClearPasswordResetToken()
    {
        PasswordResetToken = null;
        PasswordResetTokenExpiresAt = null;
    }
    
    public void VerifyEmail()
    {
        IsEmailVerified = true;
        EmailVerificationToken = null;
    }
    
    public void EnableTwoFactor(string secret)
    {
        IsTwoFactorEnabled = true;
        TwoFactorSecret = secret;
    }
    
    public void DisableTwoFactor()
    {
        IsTwoFactorEnabled = false;
        TwoFactorSecret = null;
    }
} 