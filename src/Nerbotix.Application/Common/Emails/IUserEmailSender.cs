namespace Nerbotix.Application.Common.Emails;

public interface IUserEmailSender
{
    Task SendRegistrationEmail(Domain.Tenants.User user, string token);
    Task SendResetPasswordEmail(Domain.Tenants.User user, string code);
    Task SendUserInvitationEmail(Domain.Tenants.User user, string token, string tenantName);
}