using BachBetV2.Application.Interfaces;
using BachBetV2.Application.Models;
using Microsoft.Extensions.Caching.Memory;

namespace BachBetV2.Application.Services
{
    public sealed class CachingService : ICachingService
    {
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheOptions;

        public CachingService(IMemoryCache cache)
        {
            _cache = cache;
            _cacheOptions = new MemoryCacheEntryOptions().SetSize(1);
        }

        public bool CacheIsCurrent<T>()
        {
            _cache.TryGetValue(GetCacheStatusKey<T>(), out bool upToDate);

            return upToDate;
        }

        public List<T>? GetCachedValue<T>()
        {
            _cache.TryGetValue(GetCacheValueKey<T>(), out List<T>? cachedValue);

            return cachedValue;
        }

        public void UpdateCache<T>(List<T>? newValue)
        {
            _cache.Set(GetCacheValueKey<T>(), newValue, _cacheOptions);
            SetCacheStatus<T>(true);
        }

        public void SetCacheStatus<T>(bool cacheUpToDate)
        {
            _cache.Set(GetCacheStatusKey<T>(), cacheUpToDate, _cacheOptions);
        }

        private static string GetCacheStatusKey<T>()
        {
            string cacheKey = typeof(T) switch
            {
                Type bet when bet == typeof(Bet) => "BetCacheUpToDate",
                Type challenge when challenge == typeof(Challenge) => "ChallengeCacheUpToDate",
                Type claim when claim == typeof(ChallengeClaim) => "ClaimCacheUpToDate",
                _ => string.Empty
            };

            return cacheKey;
        }

        private static string GetCacheValueKey<T>()
        {
            string cacheKey = typeof(T) switch
            {
                Type bet when bet == typeof(Bet) => "CurrentBets",
                Type challenge when challenge == typeof(Challenge) => "CurrentChallenges",
                Type claim when claim == typeof(ChallengeClaim) => "CurrentClaims",
                _ => string.Empty
            };

            return cacheKey;
        }

    }
}
