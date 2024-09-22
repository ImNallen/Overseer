using FluentEmail.Core;
using Overseer.Api.Features.Abstractions;

namespace Overseer.Api.Features.Users.Events;

public class UserRegisteredDomainEventHandler(IFluentEmail fluentEmail) : IDomainEventHandler<UserRegisteredDomainEvent>
{
    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        string body = string.Empty;

        if (notification.Token is null)
        {
            body = $@"
                <h1>Welcome to Overseer!</h1>
                <p>We're excited to have you on board.</p>
                <p>If you have any questions or need assistance, don't hesitate to contact our support team.</p>
                <p>Thank you for choosing Overseer!</p>";
        }
        else
        {
            body = $@"
                <h1>Welcome to Overseer!</h1>
                <p>We're excited to have you on board. To get started, please verify your email address by clicking the link below:</p>
                <p><a href=""http://localhost:5000/users/verify?token={notification.Token}"">Verify Your Email</a></p>
                <p>If you have any questions or need assistance, don't hesitate to contact our support team.</p>
                <p>Thank you for choosing Overseer!</p>";
        }

        await fluentEmail
            .To(notification.Email)
            .Subject("Welcome to Overseer")
            .Body(body, isHtml: true)
            .SendAsync(cancellationToken);
    }
}
