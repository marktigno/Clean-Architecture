using FluentValidation;

namespace Application.TodoEntries.Commands.DeleteTodoEntry
{
    public sealed class DeleteTodoEntryCommandValidator : AbstractValidator<DeleteTodoEntryCommand>
    {
        public DeleteTodoEntryCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}