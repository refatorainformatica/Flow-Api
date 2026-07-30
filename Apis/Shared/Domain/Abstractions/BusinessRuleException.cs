using System;

namespace Shared.Domain.Abstractions
{
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message)
            : base(message) { }
    }
}
