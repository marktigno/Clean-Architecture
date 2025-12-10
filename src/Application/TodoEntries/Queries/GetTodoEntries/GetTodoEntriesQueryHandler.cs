using Application.Abstractions.Messaging;
using Domain.Entities;
using Domain.Repositories;
using Domain.Shared;

namespace Application.TodoEntries.Queries.GetTodoEntries
{
    public sealed class GetTodoEntriesQueryHandler(IRepository repository) : IQueryHandler<GetTodoEntriesQuery, Result>
    {
        public readonly IRepository _repository = repository;

        public async Task<Result<Result>> Handle(GetTodoEntriesQuery request, CancellationToken cancellationToken)
        {
            List<TodoEntry> todoEntries;

            todoEntries = await _repository.GetTodoEntries();

            if (!todoEntries.Any())
            {
                return Result.Failure(TodoEntryError.EmptyOrNull);
            }

            return Result.Success(new GetTodoEntriesResponse(todoEntries));
        }
    }
}
