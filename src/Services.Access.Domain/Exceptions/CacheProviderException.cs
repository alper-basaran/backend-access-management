using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Access.Domain.Exceptions
{
    public class CacheProviderException : Exception
    {
        private static readonly string DefaultMessage = "An error occured during cache access";

        public CacheProviderException() : base(DefaultMessage)
        {
        }
        public CacheProviderException(string message) : base(message)
        {
        }
        public CacheProviderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
