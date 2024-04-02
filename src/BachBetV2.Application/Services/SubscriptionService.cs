using BachBetV2.Application.DTOs;
using BachBetV2.Application.Extensions;
using BachBetV2.Application.Interfaces;
using BachBetV2.Application.Models;
using BachBetV2.Application.Models.Results;

namespace BachBetV2.Application.Services
{
    public sealed class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        public SubscriptionService(ISubscriptionRepository pushSubscriptionRepository)
        {
            _subscriptionRepository = pushSubscriptionRepository;
        }

        public async Task<Result> AddSubscriptionAsync(Subscription pushSubscription, CancellationToken cancellationToken)
        {
            try
            {
                SubscriptionDto dto = new()
                {
                    UserId = pushSubscription.UserId!,
                    Auth = pushSubscription.Auth,
                    P256DH = pushSubscription.P256DH,
                    PushEndpoint = pushSubscription.PushEndpoint,
                    PublicKey = pushSubscription.PublicKey,
                    PrivateKey = pushSubscription.PrivateKey,
                };

                await _subscriptionRepository.AddPushSubscription(dto, cancellationToken);

                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return ex.HandleError();
            }
        }
    }
}
