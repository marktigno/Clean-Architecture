
using Application.Abstractions.Messaging;
using Application.TodoEntries.Queries.GetTodoEntries;
using Domain.Shared;
using WebApi.Extensions;

namespace WebApi.Endpoints.Todos
{
    internal sealed class GetAll : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/todos", async (IQueryHandler<GetTodoEntriesQuery, Result> getAllTodoEntriesQueryHandler, CancellationToken cancellationToken) =>
            {
                var query = new GetTodoEntriesQuery();
                var result = await getAllTodoEntriesQueryHandler.Handle(query, cancellationToken);

                if (result.Value.IsFailure)
                {
                    return Results.BadRequest(result.Value.ToProblemDetails());
                }

                return Results.Ok(result.Value);
            })
                .Produces<GetTodoEntriesResponse>(StatusCodes.Status200OK)
                .WithTags("Todos")
                .WithName("GetAllTodos");
        }
    }
}
