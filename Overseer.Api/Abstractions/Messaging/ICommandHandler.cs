using MediatR;
using Overseer.Api.Features.Abstractions;

namespace Overseer.Api.Abstractions.Messaging;

public interface ICommandHandler<in TCommand>
    : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}

public interface ICommandHandler<in TCommand, TResponse>
    : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}
