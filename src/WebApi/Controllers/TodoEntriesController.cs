using Application.Abstractions.Messaging;
using Application.TodoEntries.Commands.CreateTodoEntry;
using Application.TodoEntries.Commands.DeleteTodoEntry;
using Application.TodoEntries.Commands.UpdateTodoEntry;
using Application.TodoEntries.Queries.GetTodoEntries;
using Application.TodoEntries.Queries.GetTodoEntryById;
using Domain.Shared;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    public sealed class TodoEntriesController(
        ICommandHandler<CreateTodoEntryCommand> createTodoEntryCommandHandler,
        ICommandHandler<UpdateTodoEntryCommand> updateTodoEntryCommandHandler,
        ICommandHandler<DeleteTodoEntryCommand> deleteTodoEntryCommandHandler,
        IQueryHandler<GetTodoEntryByIdQuery, Result> getTodoEntryByIdQueryHandler,
        IQueryHandler<GetTodoEntriesQuery, Result> getTodoEntriesQueryHandler) : ApiController
    {
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(GetTodoEntryByIdResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTodoEntryById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetTodoEntryByIdQuery(id);
            var result = await getTodoEntryByIdQueryHandler.Handle(query, cancellationToken);

            if (result.Value.IsFailure)
            {
                return NotFound(result.Value.ToProblemDetails());
            }

            return Ok(result.Value);
        }

        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteTodoEntry(DeleteTodoEntryRequest request, CancellationToken cancellationToken)
        {
            var query = new GetTodoEntryByIdQuery(request.Id);
            var existingTodoEntry = await getTodoEntryByIdQueryHandler.Handle(query, cancellationToken);

            if (existingTodoEntry.Value.IsFailure)
            {
                return NotFound(existingTodoEntry.Value.ToProblemDetails());
            }

            var command = new DeleteTodoEntryCommand(request.Id);
            var result = await deleteTodoEntryCommandHandler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.ToProblemDetails());
            }

            return Ok(result);
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTodoEntry(UpdateTodoEntryRequest request, CancellationToken cancellationToken)
        {
            var query = new GetTodoEntryByIdQuery(request.Id);
            var existingTodoEntry = await getTodoEntryByIdQueryHandler.Handle(query, cancellationToken);

            if (existingTodoEntry.Value.IsFailure)
            {
                return NotFound(existingTodoEntry.Value.ToProblemDetails());
            }

            var todoUpdateRequest = Todo.Create(request.Todo);

            if (todoUpdateRequest.IsSuccess)
            {
                var command = new UpdateTodoEntryCommand(request.Id, todoUpdateRequest.Value);
                var result = await updateTodoEntryCommandHandler.Handle(command, cancellationToken);

                if (result.IsFailure)
                {
                    return BadRequest(result.ToProblemDetails());
                }

                return Ok(result);
            }
            return BadRequest(todoUpdateRequest.ToProblemDetails());
        }


        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateTodoEntry(CreateTodoEntryRequest request, CancellationToken cancellationToken)
        {
            var todoRequest = Todo.Create(request.Todo);

            if (todoRequest.IsSuccess)
            {
                var command = new CreateTodoEntryCommand(todoRequest.Value);
                var result = await createTodoEntryCommandHandler.Handle(command, cancellationToken);

                if (result.IsFailure)
                {
                    return BadRequest(result.ToProblemDetails());
                }

                return Ok(result);
            }

            return BadRequest(todoRequest.ToProblemDetails());
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTodoEntries(CancellationToken cancellationToken)
        {
            var query = new GetTodoEntriesQuery();
            var result = await getTodoEntriesQueryHandler.Handle(query, cancellationToken);

            if (result.Value.IsFailure)
            {
                return BadRequest(result.Value.ToProblemDetails());
            }

            return Ok(result.Value);
        }
    }
}
