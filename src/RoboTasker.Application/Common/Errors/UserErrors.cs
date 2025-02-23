namespace RoboTasker.Application.Common.Errors;

public static class UserErrors
{
    public const string RegisterFailed = "Register.Failed";
    public const string RegisterFailedDescription = "Error while registering user. See details.";

    public const string NotFound = "User.NotFound";
    public const string NotFoundDescription = "User was not found.";
    
    public const string InvalidPassword = "Password.Invalid";
    public const string InvalidPasswordDescription = "Invalid password";
}