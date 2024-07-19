using Application.Abstractions.Messaging;
using Domain.Entities;
using Domain.Repositories;
using Domain.Shared;

namespace Application.TodoEntries.Commands.CreateTodoEntry
{
    public sealed class CreateTodoEntryCommandHandler : ICommandHandler<CreateTodoEntryCommand, Result>
    {
        private readonly IRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTodoEntryCommandHandler(IRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(CreateTodoEntryCommand command, CancellationToken cancellationToken)
        {
            var todoEntry = new TodoEntry(Guid.NewGuid(), command.Todo);

            if (command.Todo == null)
            {
                return Result.Failure(TodoEntryError.EmptyOrNull);
            }

            await _repository.AddTodo(todoEntry);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(todoEntry);
        }
    }
}
