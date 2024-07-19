using Application.TodoEntries.Commands.CreateTodoEntry;
using Application.TodoEntries.Queries.GetTodoEntryById;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    public sealed class TodoEntriesController : ApiController
    {
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(TodoEntryResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTodoEntryById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetTodoEntryByIdQuery(id);

            var todoEntry = await Sender.Send(query, cancellationToken);

            if (todoEntry.IsFailure)
            {
                return NotFound(todoEntry.ToProblemDetails());
            }

            return Ok(todoEntry);
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTodoEntry(CreateTodoEntryRequest request, CancellationToken cancellationToken)
        {
            var todoRequest = Todo.Create(request.todo);
            if (todoRequest.IsSuccess)
            {
                var command = new CreateTodoEntryCommand(todoRequest.Value);

                var todoEntry = await Sender.Send(command, cancellationToken);

                if (todoEntry.IsFailure)
                {
                    return BadRequest(todoEntry.ToProblemDetails());
                }

                return Ok(todoEntry);
            }

            return BadRequest(todoRequest.ToProblemDetails());
        }
    }
}
