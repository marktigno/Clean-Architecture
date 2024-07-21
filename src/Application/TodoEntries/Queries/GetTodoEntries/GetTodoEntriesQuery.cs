using Application.Abstractions.Messaging;
using Domain.Shared;

namespace Application.TodoEntries.Queries.GetTodoEntries
{
    public sealed record GetTodoEntriesQuery() : IQuery<Result>
    {
    }
}
