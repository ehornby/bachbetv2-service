using BachBetV2.Application.Models;
using BachBetV2.Application.Models.Results;

namespace BachBetV2.Application.Interfaces
{
    public interface IPushNotificationService
    {
        Task SendPushNotificationAsync(PushNotification pushNotification, CancellationToken cancellationToken);
        Task<Result> SendManualPushNotificationAsync(PushNotification pushNotification, CancellationToken cancellationToken);

    }
}