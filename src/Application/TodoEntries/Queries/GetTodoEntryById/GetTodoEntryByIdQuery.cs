using Application.Abstractions.Messaging;
using Domain.Shared;

namespace Application.TodoEntries.Queries.GetTodoEntryById
{
    public sealed record GetTodoEntryByIdQuery(Guid Id) : IQuery<Result>;
}
