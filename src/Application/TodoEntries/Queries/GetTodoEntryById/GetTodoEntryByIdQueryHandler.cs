using Application.Abstractions.Messaging;
using Domain.Entities;
using Domain.Repositories;
using Domain.Shared;

namespace Application.TodoEntries.Queries.GetTodoEntryById
{
    public sealed class GetTodoEntryByIdQueryHandler(IRepository repository) : IQueryHandler<GetTodoEntryByIdQuery, Result>
    {
        public readonly IRepository _repository = repository;

        public async Task<Result> Handle(GetTodoEntryByIdQuery request, CancellationToken cancellationToken)
        {
            TodoEntry? todo = await _repository.GetTodoEntryById(request.Id);

            if (todo is null)
            {
                return Result.Failure(TodoEntryError.NotFoundEntry);
            }

            return Result.Success(new GetTodoEntryByIdResponse(todo));
        }
    }
}
