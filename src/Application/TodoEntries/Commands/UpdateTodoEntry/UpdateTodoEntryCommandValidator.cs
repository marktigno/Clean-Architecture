using FluentValidation;

namespace Application.TodoEntries.Commands.UpdateTodoEntry
{
    public sealed class UpdateTodoEntryCommandValidator : AbstractValidator<UpdateTodoEntryCommand>
    {
        public UpdateTodoEntryCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
            RuleFor(x => x.Todo).NotNull().NotEmpty();
        }
    }
}