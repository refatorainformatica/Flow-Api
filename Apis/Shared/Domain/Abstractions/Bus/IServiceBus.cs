using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Domain.Abstractions.Bus
{
    public interface IServiceBus
    {
        Task SendAsync(
            string connectionString,
            string queueOrTopicName,
            string message,
            Dictionary<string, object> messageProperties = null
        );

        Task ScheduleMessageAsync(
            string connectionString,
            string queueOrTopicName,
            string message,
            DateTimeOffset publishOn,
            Dictionary<string, object> messageProperties = null
        );

        Task SendBatchAsync(
            string connectionString,
            string queueOrTopicName,
            List<string> messages
        );
    }
}
