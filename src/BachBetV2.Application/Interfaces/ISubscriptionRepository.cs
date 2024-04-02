using BachBetV2.Application.DTOs;

namespace BachBetV2.Application.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task AddPushSubscription(SubscriptionDto dto, CancellationToken cancellationToken);
        Task<List<SubscriptionDto>?> GetAllPushSubscriptions(CancellationToken cancellationToken);
        Task<SubscriptionDto?> GetPushSubscriptionByUser(string userId, CancellationToken cancellationToken);
    }
}