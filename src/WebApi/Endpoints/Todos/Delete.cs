using Application.Abstractions.Messaging;
using Application.TodoEntries.Commands.DeleteTodoEntry;
using Application.TodoEntries.Queries.GetTodoEntryById;
using Domain.Shared;
using Domain.ValueObjects;
using WebApi.Extensions;

namespace WebApi.Endpoints.Todos
{
    internal sealed class Delete : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("/todos/delete", async ([AsParameters] DeleteTodoEntryRequest request, ICommandHandler<DeleteTodoEntryCommand> deleteTodoEntryCommandHandler, IQueryHandler<GetTodoEntryByIdQuery, Result> getTodoEntryByIdQueryHandler, CancellationToken cancellationToken) =>
            {
                var query = new GetTodoEntryByIdQuery(request.Id);
                var existingTodoEntry = await getTodoEntryByIdQueryHandler.Handle(query, cancellationToken);

                if (existingTodoEntry.Value.IsFailure)
                {
                    return Results.NotFound(existingTodoEntry.Value.ToProblemDetails());
                }

                var command = new DeleteTodoEntryCommand(request.Id);
                var result = await deleteTodoEntryCommandHandler.Handle(command, cancellationToken);

                if (result.IsFailure)
                {
                    return Results.BadRequest(result.ToProblemDetails());
                }

                return Results.Ok(result);
            })
            .Produces(StatusCodes.Status200OK)
            .WithName("DeleteTodo")
            .WithTags("Todos");
        }
    }
}