using System;
using Shared.Domain.Abstractions.DateTime;

namespace Shared.Infrastructure
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
