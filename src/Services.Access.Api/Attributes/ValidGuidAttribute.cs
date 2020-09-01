using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

//Based on: https://andrewlock.net/creating-an-empty-guid-validation-attribute/
namespace Services.Access.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Property 
        | AttributeTargets.Field 
        | AttributeTargets.Parameter, AllowMultiple = false)]
    public class ValidGuidAttribute : ValidationAttribute
    {
        public const string DefaultErrorMessage = "The {0} field must not be empty";
        private readonly bool _isRequired;
        public ValidGuidAttribute(bool required = false) : base(DefaultErrorMessage) 
        {
            _isRequired = required;
        }

        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return !_isRequired;
            }

            switch (value)
            {
                case Guid guid:
                    return guid != Guid.Empty;
                default:
                    return false;
            }
        }
    }
}
