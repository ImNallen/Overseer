using Overseer.Api.Features.Abstractions;

namespace Overseer.Api.Features.Organisations.Entities;

public static class OrganisationErrors
{
    public static Error NotFound => Error.NotFound("Organisation.NotFound", "No organisation found.");
}
