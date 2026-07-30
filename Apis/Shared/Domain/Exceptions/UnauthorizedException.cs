using System;
using System.Globalization;

namespace Shared.Domain.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
            : base() { }

        public UnauthorizedException(string message)
            : base(message) { }

        public UnauthorizedException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args)) { }
    }
}
