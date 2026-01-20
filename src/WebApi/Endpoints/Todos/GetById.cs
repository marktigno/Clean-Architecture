using Application.Abstractions.Messaging;
using Application.TodoEntries.Queries.GetTodoEntryById;
using Domain.Shared;
using WebApi.Extensions;

namespace WebApi.Endpoints.Todos
{
    internal sealed class GetById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/todos/{id:guid}", async (Guid id, IQueryHandler<GetTodoEntryByIdQuery, Result> getTodoEntryByIdQueryHandler, CancellationToken cancellationToken) =>
            {
                var query = new GetTodoEntryByIdQuery(id);
                var result = await getTodoEntryByIdQueryHandler.Handle(query, cancellationToken);
                if (result.Value.IsFailure)
                {
                    return Results.NotFound(result.Value.ToProblemDetails());
                }
                return Results.Ok(result.Value);
            })
            .Produces<GetTodoEntryByIdResponse>(StatusCodes.Status200OK)
            .WithName("GetTodoById")
            .WithTags("Todos");
        }
    }
}
