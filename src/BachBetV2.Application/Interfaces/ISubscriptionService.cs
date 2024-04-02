using BachBetV2.Application.Models;
using BachBetV2.Application.Models.Results;

namespace BachBetV2.Application.Interfaces
{
    public interface ISubscriptionService
    {
        Task<Result> AddSubscriptionAsync(Subscription pushSubscription, CancellationToken cancellationToken);
    }
}