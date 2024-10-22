using System.Globalization;
using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Users.Entities;
using Overseer.Api.Persistence;
using Overseer.Api.Utilities.Exceptions;

namespace Overseer.Api.Features.Users;

public record GetUsersQuery(
    string? SearchTerm,
    string? SortColumn,
    string? SortOrder,
    int Page,
    int PageSize) : IQuery<PagedList<User>>;

public record UserResponse(
    Guid Id,
    string Email,
    string Username,
    string FirstName,
    string LastName,
    string Role,
    bool IsEmailVerified,
    DateTime CreatedAt);

public class GetUsersEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/users", async (
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            int page,
            int pageSize,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetUsersQuery(
                searchTerm,
                sortColumn,
                sortOrder,
                page,
                pageSize);

            Result<PagedList<User>> result = await sender.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return CustomResults.Problem(result);
            }

            return Results.Ok(PagedList<UserResponse>
                .Create(
                    result.Value.Items.Select(x =>
                    new UserResponse(
                        x.Id,
                        x.Email.Value,
                        x.Username.Value,
                        x.FirstName.Value,
                        x.LastName.Value,
                        x.Roles.First().Name,
                        x.IsEmailVerified,
                        x.CreatedAt)).ToList(),
                    result.Value.Page,
                    result.Value.PageSize,
                    result.Value.TotalCount));
        })
        .WithTags(Tags.Users)
        .RequireAuthorization(Permissions.UsersRead);
}

public class GetUsersHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetUsersQuery, PagedList<User>>
{
    public async Task<Result<PagedList<User>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        IQueryable<User> query = unitOfWork.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query
                .Where(x =>
                    x.Id.ToString().Contains(request.SearchTerm) ||
                    ((string)x.Email).Contains(request.SearchTerm) ||
                    ((string)x.Username).Contains(request.SearchTerm) ||
                    ((string)x.FirstName + " " + (string)x.LastName).Contains(request.SearchTerm));
        }

        Expression<Func<User, object>> keySelector = request.SortColumn?.ToLower(CultureInfo.CurrentCulture) switch
        {
            "email" => c => c.Email,
            "username" => c => c.Username,
            "createdat" => c => c.CreatedAt,
            _ => c => c.CreatedAt
        };

        query = request.SortOrder?.ToLower(CultureInfo.CurrentCulture) == "desc"
            ? query.OrderByDescending(keySelector)
            : query.OrderBy(keySelector);

        int totalCount = await query.CountAsync(cancellationToken);

        List<User> users = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Include(x => x.Roles)
            .ToListAsync(cancellationToken);

        return PagedList<User>.Create(users, request.Page, request.PageSize, totalCount);
    }
}
