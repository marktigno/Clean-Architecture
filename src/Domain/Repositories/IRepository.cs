using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Domain.Repositories
{
    public interface IRepository
    {
        Task AddTodo(TodoEntry todoEntry);

        void UpdateTodo(TodoEntry todoEntry);

        void DeleteTodo(TodoEntry todoEntry);

        Task<List<TodoEntry>> GetTodoEntries();

        Task<TodoEntry?> GetTodoEntryById(Guid id);
    }
}
