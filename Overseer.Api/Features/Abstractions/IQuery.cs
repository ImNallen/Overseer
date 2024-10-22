using MediatR;

namespace Overseer.Api.Features.Abstractions;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
