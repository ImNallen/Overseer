using FluentEmail.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Overseer.Api.Abstractions.Exceptions;
using Overseer.Api.Abstractions.Messaging;
using Overseer.Api.Abstractions.Persistence;
using Overseer.Api.Abstractions.Time;
using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Users.Entities;

namespace Overseer.Api.Features.Users;

public record ForgotPasswordRequest(string Email);

public record ForgotPasswordCommand(string Email) : ICommand;

public class ForgotPasswordEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPost("/users/forgot", async (
            ForgotPasswordRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new ForgotPasswordCommand(request.Email);

            Result result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return CustomResults.Problem(result);
            }

            return Results.Ok();
        })
        .WithTags(Tags.Users)
        .AllowAnonymous();
}

public class ForgotPasswordHandler(
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    IFluentEmail emailService) : ICommandHandler<ForgotPasswordCommand>
{
    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        User? user = await unitOfWork.Users.FirstOrDefaultAsync(x => (string)x.Email == request.Email, cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        user.SetPasswordResetToken(Guid.NewGuid(), dateTimeProvider.UtcNow.AddDays(1));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await emailService
            .To(user.Email.Value)
            .Subject("Reset Your Password | Overseer")
            .Body(
                string.Join(
                Environment.NewLine,
                $"Dear {user.FirstName.Value},",
                "<br/><br/>",
                "You have requested to reset your password. Please click the link below to reset your password:",
                "<br/><br/>",
                $"<a href=\"https://localhost:5000/reset/{user.PasswordResetToken}\">Reset Password</a>",
                "<br/><br/>",
                "This link will expire in 24 hours.",
                "<br/><br/>",
                "If you did not request a password reset, please ignore this email. Or contact support if you believe this was a mistake.",
                "<br/><br/>",
                "Best regards,",
                "<br/><br/>",
                "The Overseer Team"),
                isHtml: true)
            .SendAsync(cancellationToken);

        return Result.Success();
    }
}
