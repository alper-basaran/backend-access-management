using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Access.Domain.Exceptions
{
    public class LockNotFoundException : Exception
    {
        private static readonly string DefaultMessage = "The specified lock could not be found";

        public LockNotFoundException() : base(DefaultMessage)
        {
        }
        public LockNotFoundException(Guid id) : base($"The lock with specified id: {id} could not be found")
        {
        }
        public LockNotFoundException(string message) : base(message)
        {
        }
        public LockNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
