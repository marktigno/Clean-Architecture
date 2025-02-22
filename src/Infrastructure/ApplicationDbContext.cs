using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public sealed class ApplicationDbContext(DbContextOptions options) : DbContext(options), IUnitOfWork
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            // modelBuilder.Entity<TodoEntry>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
