namespace RoboTasker.Application.Common.Errors;

public static class UserErrors
{
    public const string ChangePasswordFailed = "ChangePassword.Failed";
    public const string ChangePasswordFailedDescription = "Error while changing the password. See details.";

    public const string EmailUnverified = "Email.Unverified";
    public const string EmailUnverifiedDescription = "You did not confirm your email address. Please check your email address.";
    
    public const string NotFound = "User.NotFound";
    public const string NotFoundDescription = "User was not found.";
    
    public const string InvalidPassword = "Password.Invalid";
    public const string InvalidPasswordDescription = "Invalid password";
    
    public const string Conflict = "User.Conflict";
    public const string ConflictDescription = "User with this email already exists.";
    
    public const string FailureWhileCreating = "User.CreationFailed";
    public const string FailureWhileCreatingDescription = "Cannot create a user. Please try again.";
    
    public const string SuperAdminDelete = "SuperAdmin.Delete";
    public const string SuperAdminDeleteDescription = "You cannot delete a super admin user.";
}