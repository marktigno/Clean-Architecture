using Application.Abstractions.Messaging;
using Domain.ValueObjects;

namespace Application.TodoEntries.Commands.DeleteTodoEntry
{
    public sealed record DeleteTodoEntryCommand(Guid Id) : ICommand;
}