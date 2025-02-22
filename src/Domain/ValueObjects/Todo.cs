using Domain.Primitives;
using Domain.Shared;

namespace Domain.ValueObjects
{
    public sealed class Todo : ValueObject
    {
        public const int MaxLength = 100;

        private Todo()
        {
            Value = string.Empty; // Initialize Value to avoid CS8618
        }

        private Todo(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<Todo> Create(string todo)
        {
            if (string.IsNullOrWhiteSpace(todo))
            {
                return Result.Failure<Todo>(new Error(
                    "Todo.Empty",
                    "Todo item is empty.",
                    ErrorType.Validation));
            }

            if (todo.Length > MaxLength)
            {
                return Result.Failure<Todo>(new Error(
                    "Todo.TooLong",
                    $"Todo item is too long (max characters: 100, todo entry: {todo.Length}.",
                    ErrorType.Validation));
            }

            return new Todo(todo);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
