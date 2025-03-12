namespace RoboTasker.Application.Common.Emails;

public interface IUserEmailSender
{
    Task SendRegistrationEmail(Domain.Tenants.User user, string token);
}