using System;
using System.Collections.Generic;
using System.Text;
using static Services.Access.Domain.Model.Enums;

namespace Services.Access.Domain.Exceptions
{
    public class InsufficientPermissionException : Exception
    {
        private static readonly string DefaultMessage = "The user does not haave sufficient permissions against this object";

        public InsufficientPermissionException() : base(DefaultMessage)
        {
        }
        public InsufficientPermissionException(Guid userId, Guid objectId, PermissionLevel requiredLevel) 
            : base($"User {userId} does not have sufficient permissions against object {objectId},"+
                  $" minimum required permission is {Enum.GetName(typeof(PermissionLevel),requiredLevel) }")
        {
        }
        public InsufficientPermissionException(string message) : base(message)
        {
        }
        public InsufficientPermissionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
