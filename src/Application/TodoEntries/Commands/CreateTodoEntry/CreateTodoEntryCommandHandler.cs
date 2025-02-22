using Application.Abstractions.Messaging;
using Domain.Entities;
using Domain.Repositories;
using Domain.Shared;

namespace Application.TodoEntries.Commands.CreateTodoEntry
{
    public sealed class CreateTodoEntryCommandHandler(IRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<CreateTodoEntryCommand, Result>
    {
        private readonly IRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(CreateTodoEntryCommand command, CancellationToken cancellationToken)
        {
            var todoEntry = new TodoEntry(Guid.NewGuid(), command.Todo);

            if (command.Todo == null)
            {
                return Result.Failure(TodoEntryError.EmptyOrNull);
            }

            await _repository.AddTodo(todoEntry);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(new CreateTodoEntryResponse(todoEntry.Id));
        }
    }
}
