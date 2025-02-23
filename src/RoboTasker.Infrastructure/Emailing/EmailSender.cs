using FluentEmail.Core;
using RoboTasker.Application.Services;

namespace RoboTasker.Infrastructure.Emailing;

public class EmailSender(IFluentEmail fluentEmail) : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string message, bool isHtml = false, CancellationToken cancellationToken = default)
    {
        await fluentEmail
            .To(email)
            .Subject(subject)
            .Body(message, isHtml)
            .SendAsync(cancellationToken);
    }
}