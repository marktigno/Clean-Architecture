using FluentValidation;

namespace Application.TodoEntries.Commands.CreateTodoEntry
{
    public sealed class CreateTodoEntryCommandValidator : AbstractValidator<CreateTodoEntryCommand>
    {
        public CreateTodoEntryCommandValidator()
        {
            RuleFor(x => x.Todo).NotNull().NotEmpty();
        }
    }
}
