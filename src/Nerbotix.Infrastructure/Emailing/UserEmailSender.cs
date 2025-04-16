using Microsoft.Extensions.Configuration;
using Nerbotix.Application.Common.Emails;
using Nerbotix.Application.Services;
using Nerbotix.Domain.Tenants;

namespace Nerbotix.Infrastructure.Emailing;

public class UserEmailSender(IConfiguration configuration, TemplateService templateService, IEmailSender emailSender) : IUserEmailSender
{
    public async Task SendRegistrationEmail(User user, string token)
    {
        var frontendUrl = configuration.GetConnectionString("FrontendUrl");
        var link = new Uri($"{frontendUrl}/auth/register?email={user.Email}&token={token}");
        var placeholders = new Dictionary<string, string>()
        {
            { "link", link.ToString() }
        };
        
        var content = await templateService.RenderTemplate(TemplateNames.Register, placeholders);
        
        await emailSender.SendEmailAsync(user.Email!, "Welcome to Nerbotix!", content, true);
    }

    public async Task SendResetPasswordEmail(User user, string code)
    {
        var placeholders = new Dictionary<string, string>()
        {
            { "code", code }
        };
        
        var content = await templateService.RenderTemplate(TemplateNames.ForgotPassword, placeholders);
        await emailSender.SendEmailAsync(user.Email!, "Password Reset", content, true);
    }

    public async Task SendUserInvitationEmail(User user, string token, string tenantName)
    {
        var frontendUrl = configuration.GetConnectionString("FrontendUrl");
        var link = new Uri($"{frontendUrl}/auth/register?email={user.Email}&token={token}");
        var placeholders = new Dictionary<string, string>()
        {
            { "link", link.ToString() },
            { "tenantName", tenantName }
        };
        
        var content = await templateService.RenderTemplate(TemplateNames.InviteUser, placeholders);
        await emailSender.SendEmailAsync(user.Email!, "Complete registration", content, true);
    }
}