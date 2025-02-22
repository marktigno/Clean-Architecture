using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public sealed class TodoEntryRepository(ApplicationDbContext dbContext) : IRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task AddTodo(TodoEntry todoEntry) => await _dbContext.AddAsync(todoEntry);

        public Task<List<TodoEntry>> GetTodoEntries() => _dbContext.Set<TodoEntry>().AsNoTracking().ToListAsync();

        public Task<TodoEntry?> GetTodoEntryById(Guid id) => _dbContext.Set<TodoEntry>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }
}
