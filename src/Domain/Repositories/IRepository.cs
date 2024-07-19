using Domain.Entities;

namespace Domain.Repositories
{
    public interface IRepository
    {
        Task AddTodo(TodoEntry todoEntry);

        Task<List<TodoEntry>> GetTodoEntries();

        Task<TodoEntry?> GetTodoEntryById(Guid id);
    }
}
