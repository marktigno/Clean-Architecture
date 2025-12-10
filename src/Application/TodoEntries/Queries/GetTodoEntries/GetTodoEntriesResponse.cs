using Domain.Entities;

namespace Application.TodoEntries.Queries.GetTodoEntries
{
    public sealed record GetTodoEntriesResponse(List<TodoEntry> TodoEntries);
}
