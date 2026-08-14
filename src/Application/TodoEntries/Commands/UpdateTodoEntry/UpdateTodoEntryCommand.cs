using Application.Abstractions.Messaging;
using Domain.ValueObjects;

namespace Application.TodoEntries.Commands.UpdateTodoEntry
{
    public sealed record UpdateTodoEntryCommand(Guid Id, Todo Todo) : ICommand;
}