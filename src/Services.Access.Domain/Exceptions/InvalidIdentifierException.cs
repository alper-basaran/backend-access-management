using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Access.Domain.Exceptions
{
    public class InvalidIdentifierException : Exception
    {
        private static readonly string DefaultMessage = "The specified value is not valid, it must be a valid GUID";

        public InvalidIdentifierException() : base(DefaultMessage)
        {
        }
        public InvalidIdentifierException(Guid id) : base($"The specified value {id} is not valid, it must be a valid GUID")
        {
        }
        public InvalidIdentifierException(string message) : base(message)
        {
        }
        public InvalidIdentifierException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
