using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public sealed class TodoEntry : Entity
    {
        private TodoEntry() { }
        public TodoEntry(Guid id, Todo todo) : base(id)
        {
            Todo = todo;
        }

        public Todo? Todo { get; private set; }
    }
}
