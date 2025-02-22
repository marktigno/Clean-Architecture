namespace Application.Exceptions
{
    public sealed class ValidationException(Dictionary<string, string[]> errors) : Exception("Validation errors occurred")
    {
        public Dictionary<string, string[]> Errors { get; } = errors;
    }
}
