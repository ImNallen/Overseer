using MediatR;
using Overseer.Api.Features.Abstractions;

namespace Overseer.Api.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
