using MediatR;
using Overseer.Api.Features.Abstractions;

namespace Overseer.Api.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
