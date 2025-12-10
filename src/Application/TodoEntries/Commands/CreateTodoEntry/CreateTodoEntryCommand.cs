using Application.Abstractions.Messaging;
using Domain.Shared;
using Domain.ValueObjects;

namespace Application.TodoEntries.Commands.CreateTodoEntry
{
    public sealed record CreateTodoEntryCommand(Todo Todo) : ICommand;
}
