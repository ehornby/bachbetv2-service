using BachBetV2.Application.DTOs;
using BachBetV2.Application.Interfaces;
using BachBetV2.Application.Models;
using BachBetV2.Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BachBetV2.Infrastructure.Repositories
{
    public sealed class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly BachBetContext _context;
        public SubscriptionRepository(BachBetContext context)
        {
            _context = context;
        }

        public async Task<SubscriptionDto?> GetPushSubscriptionByUser(string userId, CancellationToken cancellationToken)
        {
            var subscription = await _context.PushSubscriptions.FirstOrDefaultAsync(x => x.UserId == int.Parse(userId), cancellationToken);

            if (subscription is not null)
            {
                return new()
                {
                    UserId = subscription.UserId.ToString(),
                    Auth = subscription.Auth,
                    P256DH = subscription.P256DH,
                    PrivateKey = subscription.PrivateKey,
                    PublicKey = subscription.PublicKey,
                    PushEndpoint = subscription.PushEndpoint
                };
            }

            return null;
        }

        public async Task<List<SubscriptionDto>?> GetAllPushSubscriptions(CancellationToken cancellationToken)
        {
            var subscriptions = await _context.PushSubscriptions.Select(ps => new SubscriptionDto()
            {
                UserId = ps.UserId.ToString(),
                Auth = ps.Auth,
                P256DH = ps.P256DH,
                PrivateKey = ps.PrivateKey,
                PublicKey = ps.PublicKey,
                PushEndpoint = ps.PushEndpoint
            }).ToListAsync(cancellationToken);

            return subscriptions;
        }


        public async Task AddPushSubscription(SubscriptionDto dto, CancellationToken cancellationToken)
        {
            var subscriptionEntity = await _context.PushSubscriptions.FirstOrDefaultAsync(x => x.UserId == int.Parse(dto.UserId), cancellationToken);

            if (subscriptionEntity is null)
            {
                subscriptionEntity = new()
                {
                    UserId = int.Parse(dto.UserId!),
                    Auth = dto.Auth,
                    P256DH = dto.P256DH,
                    PrivateKey = dto.PrivateKey,
                    PublicKey = dto.PublicKey,
                    PushEndpoint = dto.PushEndpoint,
                };
            }
            else
            {
                subscriptionEntity.Auth = dto.Auth;
                subscriptionEntity.P256DH = dto.P256DH;
                subscriptionEntity.PrivateKey = dto.PrivateKey;
                subscriptionEntity.PublicKey = dto.PublicKey;
                subscriptionEntity.PushEndpoint = dto.PushEndpoint;
            }

            _context.Update(subscriptionEntity);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
