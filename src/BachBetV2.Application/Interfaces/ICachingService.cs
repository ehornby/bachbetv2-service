namespace BachBetV2.Application.Interfaces
{
    public interface ICachingService
    {
        bool CacheIsCurrent<T>();
        List<T>? GetCachedValue<T>();
        void UpdateCache<T>(List<T>? newValue);
        void SetCacheStatus<T>(bool cacheUpToDate);
    }
}