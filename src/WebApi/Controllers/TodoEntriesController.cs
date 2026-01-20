using Application.Abstractions.Messaging;
using Application.TodoEntries.Commands.CreateTodoEntry;
using Application.TodoEntries.Queries.GetTodoEntries;
using Application.TodoEntries.Queries.GetTodoEntryById;
using Domain.Shared;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    public sealed class TodoEntriesController(ICommandHandler<CreateTodoEntryCommand> createTodoEntryCommandHandler,
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

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTodoEntry(CancellationToken cancellationToken)
        {
            await Task.Delay(500, cancellationToken);
            return Ok();
        }

        [HttpDelete("delete/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteTodoEntry(CancellationToken cancellationToken)
        {
            await Task.Delay(500, cancellationToken);
            return Ok();
        }
    }
}
