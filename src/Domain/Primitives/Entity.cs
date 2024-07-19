namespace Domain.Primitives
{
    public abstract class Entity : IEquatable<Entity>
    {
        protected Entity() { }
        protected Entity(Guid id)
        {
            Id = id;
            CreatedDateTime = DateTime.UtcNow;
            // CreatedBy = createdBy;
        }

        public Guid Id { get; private init; }
        public DateTime CreatedDateTime { get; private init; }
        // public string CreatedBy { get; private init; }
        public DateTime? ModifiedDateTime { get; private set; }
        // public string? ModifiedBy { get; private set; } = string.Empty;

        public static bool operator ==(Entity? first, Entity? second)
        {
            return first is not null && second is not null && first.Equals(second);
        }

        public static bool operator !=(Entity? first, Entity? second)
        {
            return !(first == second);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            if (obj is not Entity entity)
            {
                return false;
            }

            return entity.Id == Id;
        }

        public bool Equals(Entity? other)
        {
            if (other is null)
            {
                return false;
            }

            if (other.GetType() != GetType())
            {
                return false;
            }

            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
