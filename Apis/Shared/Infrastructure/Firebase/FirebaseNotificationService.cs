using System.Collections.Generic;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shared.Domain.Abstractions.Notifications;

namespace Shared.Infrastructure.Firebase
{
    public class FirebaseNotificationService : IPushNotificationService
    {
        public async Task<string> SendPushNotificationAsync(
            string title,
            string body,
            string token,
            string imageUrl,
            NotificationMetadata notificationMetadata
        )
        {
            if (string.IsNullOrEmpty(token))
            {
                return string.Empty;
            }

            var message = new Message()
            {
                Apns = new ApnsConfig() { Aps = new Aps() { MutableContent = true } },
                Android = new AndroidConfig()
                {
                    Notification = string.IsNullOrEmpty(imageUrl)
                        ? new AndroidNotification() { Title = title, Body = body }
                        : new AndroidNotification()
                        {
                            Title = title,
                            Body = body,
                            ImageUrl = imageUrl,
                        },
                    Priority = Priority.High,
                },
                Token = token,
                Data = new Dictionary<string, string>()
                {
                    {
                        "notificationMetadata",
                        JsonConvert.SerializeObject(
                            notificationMetadata,
                            new JsonSerializerSettings
                            {
                                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                            }
                        )
                    },
                },
            };

            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

            return response;
        }
    }
}
