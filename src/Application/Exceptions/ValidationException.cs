﻿namespace Application.Exceptions
{
    public sealed class ValidationException : Exception
    {
        public ValidationException(Dictionary<string, string[]> errors)
            : base("Validation errors occurred") =>
            Errors = errors;

        public Dictionary<string, string[]> Errors { get; }
    }
}
