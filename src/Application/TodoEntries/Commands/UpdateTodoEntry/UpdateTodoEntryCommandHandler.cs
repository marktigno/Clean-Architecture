using Application.Abstractions.Messaging;
using Domain.Entities;
using Domain.Repositories;
using Domain.Shared;

namespace Application.TodoEntries.Commands.UpdateTodoEntry
{
    public sealed class UpdateTodoEntryCommandHandler(IRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateTodoEntryCommand>
    {
        private readonly IRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(UpdateTodoEntryCommand command, CancellationToken cancellationToken)
        {
            if (command.Todo == null)
            {
                return Result.Failure<TodoEntry>(TodoEntryError.EmptyOrNull);
            }

            var todoEntry = await _repository.GetTodoEntryById(command.Id);

            if (todoEntry is null)
            {
                return Result.Failure<TodoEntry>(TodoEntryError.NotFoundEntry);
            }

            todoEntry.Todo = command.Todo;
            todoEntry.ModifiedDateTime = DateTime.UtcNow;
            
            _repository.UpdateTodo(todoEntry);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(new UpdateTodoEntryResponse(todoEntry.Todo?.Value ?? string.Empty));
        }
    }
}