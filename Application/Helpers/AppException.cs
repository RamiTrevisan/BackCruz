using System;
using System.Globalization;

namespace Application.Exceptions
{
    /// <summary>
    /// Custom exception class for application-specific exceptions
    /// </summary>
    public class AppException : Exception
    {
        public AppException() : base() { }

        public AppException(string message) : base(message) { }

        public AppException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }

        public AppException(string message, Exception innerException) : base(message, innerException) { }
    }
}

