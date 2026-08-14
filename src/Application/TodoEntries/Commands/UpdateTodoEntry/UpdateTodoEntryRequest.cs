namespace Application.TodoEntries.Commands.UpdateTodoEntry
{
    public sealed record UpdateTodoEntryRequest(Guid Id, string Todo);
}