using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Access.Domain.Exceptions
{
    public class SiteNotFoundException : Exception
    {
        private static readonly string DefaultMessage = "The specified site could not be found";

        public SiteNotFoundException() : base(DefaultMessage)
        {
        }
        public SiteNotFoundException(Guid id) : base($"The site with specified id: {id} could not be found")
        {
        }
        public SiteNotFoundException(string message) : base(message)
        {
        }
        public SiteNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
