using System.Threading.Tasks;

namespace Shared.Domain.Abstractions.Notifications
{
    public interface IPushNotificationService : INotificationService
    {
        Task<string> SendPushNotificationAsync(
            string title,
            string body,
            string token,
            string imageUrl,
            NotificationMetadata notificationMetadata
        );
    }
}
