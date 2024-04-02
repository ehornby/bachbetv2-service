using BachBetV2.Application.Configuration;
using BachBetV2.Application.Interfaces;
using BachBetV2.Application.Services;
using BachBetV2.Application.Validation;
using BachBetV2.Domain.Models.Requests;
using BachBetV2.Infrastructure.Database.Contexts;
using BachBetV2.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebPush;

namespace BachBetV2.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DynamoDbOptions>(configuration.GetSection("Aws:DynamoDb"));
            services.Configure<AuthOptions>(configuration.GetSection("Auth"));
        }

        public static void AddServiceRegistrations(this IServiceCollection services)
        {
            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBetRepository, BetRepository>();
            services.AddScoped<ILedgerRepository, LedgerRepository>();
            services.AddScoped<IChallengeRepository, ChallengeRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<IPushNotificationService, PushNotificationService>();

            // Register services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBetService, BetService>();
            services.AddScoped<IChallengeService, ChallengeService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();

            services.AddSingleton<ICachingService, CachingService>();
            services.AddSingleton<WebPushClient>();
        }

        public static void AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BachBetContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("BachBetContext"));
            });
        }

        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateBetRequest>, CreateBetRequestValidator>();
            services.AddScoped<IValidator<TakeBetRequest>, TakeBetRequestValidator>();
            services.AddScoped<IValidator<CreateChallengeRequest>, CreateChallengeRequestValidator>();
            services.AddScoped<IValidator<CreateTagRequest>, CreateTagRequestValidator>();
            services.AddScoped<IValidator<BalanceTransferRequest>, BalanceTransferRequestValidator>();
            services.AddScoped<IValidator<PostNotificationRequest>, PostNotificationRequestValidator>();
        }
    }
}
