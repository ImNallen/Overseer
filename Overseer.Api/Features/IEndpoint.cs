namespace Overseer.Api.Features;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
