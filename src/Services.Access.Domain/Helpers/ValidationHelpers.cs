using Services.Access.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Access.Domain.Helpers
{
    public static class ValidationHelpers
    {
        public static void Validate(this Guid guid)
        {
            if (guid == null || guid == Guid.Empty)
            {
                throw new InvalidIdentifierException(guid);
            }

        }
    }
}
