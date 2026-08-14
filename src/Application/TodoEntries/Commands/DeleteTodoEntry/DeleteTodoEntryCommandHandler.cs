using Application.Abstractions.Messaging;
using Domain.Entities;
using Domain.Repositories;
using Domain.Shared;

namespace Application.TodoEntries.Commands.DeleteTodoEntry
{
    public sealed class DeleteTodoEntryCommandHandler(IRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<DeleteTodoEntryCommand>
    {
        private readonly IRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(DeleteTodoEntryCommand command, CancellationToken cancellationToken)
        {
            var todoEntry = await _repository.GetTodoEntryById(command.Id);

            if (todoEntry is null)
            {
                return Result.Failure<TodoEntry>(TodoEntryError.NotFoundEntry);
            }

            _repository.DeleteTodo(todoEntry);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}