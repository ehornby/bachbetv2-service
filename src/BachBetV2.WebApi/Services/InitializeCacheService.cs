using BachBetV2.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace BachBetV2.WebApi.Services
{
    public class InitializeCacheService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public InitializeCacheService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var cache = _serviceProvider.GetService<IMemoryCache>();
            var repository = scope.ServiceProvider.GetService<IUserRepository>();

            var users = await repository!.GetAllUsers(cancellationToken);
            var userTokens = users!.Select(u => u.Token).ToList();

            var cacheOptions = new MemoryCacheEntryOptions().SetSize(1);

            foreach (var token in userTokens)
            {
                cache!.Set(token.ToString(), string.Empty, cacheOptions);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
