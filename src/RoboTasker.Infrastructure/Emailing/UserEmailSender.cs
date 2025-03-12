using RoboTasker.Application.Common.Emails;
using RoboTasker.Application.Services;
using RoboTasker.Domain.Tenants;

namespace RoboTasker.Infrastructure.Emailing;

public class UserEmailSender(TemplateService templateService, IEmailSender emailSender) : IUserEmailSender
{
    public async Task SendRegistrationEmail(User user, string token)
    {
        var link = new Uri($"http://localhost:4200/auth/register?email={user.Email}&token={token}");
        var placeholders = new Dictionary<string, string>()
        {
            { "link", link.ToString() }
        };
        
        var content = await templateService.RenderTemplate(TemplateNames.Register, placeholders);
        
        await emailSender.SendEmailAsync(user.Email!, "Welcome to RoboTasker!", content, true);
    }
}