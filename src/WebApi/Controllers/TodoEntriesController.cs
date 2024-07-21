using Application.TodoEntries.Commands.CreateTodoEntry;
using Application.TodoEntries.Queries.GetTodoEntries;
using Application.TodoEntries.Queries.GetTodoEntryById;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    public sealed class TodoEntriesController : ApiController
    {
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(GetTodoEntryByIdResponse), StatusCodes.Status200OK)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateTodoEntry(CreateTodoEntryRequest request, CancellationToken cancellationToken)
        {
            var todoRequest = Todo.Create(request.todo);
            if (todoRequest.IsSuccess)
            {
                var command = new CreateTodoEntryCommand(todoRequest.Value);

                var todoEntryId = await Sender.Send(command, cancellationToken);

                if (todoEntryId.IsFailure)
                {
                    return BadRequest(todoEntryId.ToProblemDetails());
                }

                return Ok(todoEntryId);
            }

            return BadRequest(todoRequest.ToProblemDetails());
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTodoEntries(CancellationToken cancellationToken)
        {
            var query = new GetTodoEntriesQuery();

            var result = await Sender.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.ToProblemDetails());
            }

            return Ok(result);
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTodoEntry(CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpDelete("delete/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteTodoEntry(CancellationToken cancellationToken)
        {
            return Ok();
        }
    }
}
