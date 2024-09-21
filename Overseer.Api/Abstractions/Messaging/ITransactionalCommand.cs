using MediatR;
using Overseer.Api.Features.Abstractions;

namespace Overseer.Api.Abstractions.Messaging;

public interface ITransactionalCommand : ICommand;

public interface ITransactionalCommand<TResponse> : IRequest<Result<TResponse>>, ITransactionalCommand;
