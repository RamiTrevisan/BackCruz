// Application/Exceptions/NotFoundException.cs
using System;

namespace Application.Exceptions
{
    /// <summary>
    /// Exception for resources not found
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Resource not found") { }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }

        public NotFoundException(string name, object key)
            : base($"Entity '{name}' with ID '{key}' was not found.") { }
    }
}