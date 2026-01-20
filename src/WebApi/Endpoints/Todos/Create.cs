using Application.Abstractions.Messaging;
using Application.TodoEntries.Commands.CreateTodoEntry;
using Domain.ValueObjects;
using WebApi.Extensions;

namespace WebApi.Endpoints.Todos
{
    internal sealed class Create : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/todos/create", async (CreateTodoEntryRequest request, ICommandHandler<CreateTodoEntryCommand> createTodoEntryCommandHandler, CancellationToken cancellationToken) =>
            {
                var todoRequest = Todo.Create(request.Todo);

                if (todoRequest.IsSuccess)
                {
                    var command = new CreateTodoEntryCommand(todoRequest.Value);
                    var result = await createTodoEntryCommandHandler.Handle(command, cancellationToken);

                    if (result.IsFailure)
                    {
                        return Results.BadRequest(result.ToProblemDetails());
                    }
                    return Results.Ok(result);
                }

                return Results.BadRequest(todoRequest.ToProblemDetails());
            })
            .Produces(StatusCodes.Status200OK)
            .WithName("CreateTodo")
            .WithTags("Todos");
        }
    }
}
