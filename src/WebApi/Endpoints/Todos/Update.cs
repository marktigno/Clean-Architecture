using Application.Abstractions.Messaging;
using Application.TodoEntries.Commands.UpdateTodoEntry;
using Application.TodoEntries.Queries.GetTodoEntryById;
using Domain.Shared;
using Domain.ValueObjects;
using WebApi.Extensions;

namespace WebApi.Endpoints.Todos
{
    internal sealed class Update : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("/todos/update", async ([AsParameters] UpdateTodoEntryRequest request, ICommandHandler<UpdateTodoEntryCommand> updateTodoEntryCommandHandler, IQueryHandler<GetTodoEntryByIdQuery, Result> getTodoEntryByIdQueryHandler, CancellationToken cancellationToken) =>
            {
                var query = new GetTodoEntryByIdQuery(request.Id);
                var existingTodoEntry = await getTodoEntryByIdQueryHandler.Handle(query, cancellationToken);
                
                if (existingTodoEntry.Value.IsFailure)
                {
                    return Results.NotFound(existingTodoEntry.Value.ToProblemDetails());
                }

                var todoUpdateRequest = Todo.Create(request.Todo);

                if (todoUpdateRequest.IsSuccess)
                {
                    var command = new UpdateTodoEntryCommand(request.Id, todoUpdateRequest.Value);
                    var result = await updateTodoEntryCommandHandler.Handle(command, cancellationToken);

                    if (result.IsFailure)
                    {
                        return Results.BadRequest(result.ToProblemDetails());
                    }
                    return Results.Ok(result);
                }

                return Results.BadRequest(todoUpdateRequest.ToProblemDetails());
            })
            .Produces(StatusCodes.Status200OK)
            .WithName("UpdateTodo")
            .WithTags("Todos");
        }
    }
}