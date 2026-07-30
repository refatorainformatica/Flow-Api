using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Shared.Domain.Abstractions.Bus;

namespace Shared.Infrastructure.Bus
{
    public class ServiceBus : IServiceBus
    {
        public async Task ScheduleMessageAsync(
            string connectionString,
            string queueOrTopicName,
            string message,
            DateTimeOffset publishOn,
            Dictionary<string, object> messageProperties = null
        )
        {
            var @event = new ServiceBusMessage(Encoding.UTF8.GetBytes(message));

            if (messageProperties != null)
            {
                foreach (KeyValuePair<string, object> messageProperty in messageProperties)
                {
                    @event.ApplicationProperties.Add(messageProperty.Key, messageProperty.Value);
                }
            }

            var options = new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets,
                RetryOptions = new ServiceBusRetryOptions
                {
                    Mode = ServiceBusRetryMode.Exponential,
                    MaxRetries = 3,
                    Delay = TimeSpan.FromSeconds(1),
                    MaxDelay = TimeSpan.FromSeconds(10),
                },
            };

            var serviceBusClient = new ServiceBusClient(connectionString, options);
            var serviceBusSender = serviceBusClient.CreateSender(queueOrTopicName);

            await serviceBusSender.ScheduleMessageAsync(@event, publishOn);
        }

        public async Task SendAsync(
            string connectionString,
            string queueOrTopicName,
            string message,
            Dictionary<string, object> messageProperties = null
        )
        {
            var @event = new ServiceBusMessage(Encoding.UTF8.GetBytes(message));

            if (messageProperties != null)
            {
                foreach (KeyValuePair<string, object> messageProperty in messageProperties)
                {
                    @event.ApplicationProperties.Add(messageProperty.Key, messageProperty.Value);
                }
            }

            var options = new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets,
                RetryOptions = new ServiceBusRetryOptions
                {
                    Mode = ServiceBusRetryMode.Exponential,
                    MaxRetries = 3,
                    Delay = TimeSpan.FromSeconds(1),
                    MaxDelay = TimeSpan.FromSeconds(10),
                },
            };

            var serviceBusClient = new ServiceBusClient(connectionString, options);
            var serviceBusSender = serviceBusClient.CreateSender(queueOrTopicName);

            await serviceBusSender.SendMessageAsync(@event);
        }

        public async Task SendBatchAsync(
            string connectionString,
            string queueOrTopicName,
            List<string> messages
        )
        {
            var options = new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets,
                RetryOptions = new ServiceBusRetryOptions
                {
                    Mode = ServiceBusRetryMode.Exponential,
                    MaxRetries = 3,
                    Delay = TimeSpan.FromSeconds(1),
                    MaxDelay = TimeSpan.FromSeconds(10),
                },
            };

            var serviceBusClient = new ServiceBusClient(connectionString, options);
            var serviceBusSender = serviceBusClient.CreateSender(queueOrTopicName);

            var serviceBusMessageBatch = await serviceBusSender.CreateMessageBatchAsync();
            int messageCounter = 0;
            while (messageCounter < messages.Count)
            {
                var @event = new ServiceBusMessage(
                    Encoding.UTF8.GetBytes(messages[messageCounter])
                );

                if (serviceBusMessageBatch.TryAddMessage(@event))
                {
                    messageCounter++;
                }
                else
                {
                    await serviceBusSender.SendMessagesAsync(serviceBusMessageBatch);
                    serviceBusMessageBatch.Dispose();
                    serviceBusMessageBatch = await serviceBusSender.CreateMessageBatchAsync();
                }
            }

            if (serviceBusMessageBatch.Count > 0)
            {
                await serviceBusSender.SendMessagesAsync(serviceBusMessageBatch);
            }
            serviceBusMessageBatch.Dispose();
        }
    }
}
