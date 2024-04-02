using Microsoft.Extensions.Caching.Memory;

namespace BachBetV2.WebApi.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;

        public AuthMiddleware(RequestDelegate next, IMemoryCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var betRoute = context.Request.Path.StartsWithSegments("/v1/bets", StringComparison.OrdinalIgnoreCase);
            var userRoute = context.Request.Path.StartsWithSegments("/v1/users", StringComparison.OrdinalIgnoreCase);
            var challengeRoute = context.Request.Path.StartsWithSegments("/v1/challenges", StringComparison.OrdinalIgnoreCase);
            var tagRoute = context.Request.Path.StartsWithSegments("/v1/tags", StringComparison.OrdinalIgnoreCase);

            if (betRoute || userRoute || challengeRoute || tagRoute)
            {
                string? headerValue = context.Request.Headers["UserToken"];

                if (!Guid.TryParse(headerValue, out Guid userToken))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                    return;
                }

                var isValidToken = ValidateUserToken(userToken);

                if (!isValidToken)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                    return;
                }
            }

            await _next(context);
        }

        private bool ValidateUserToken(Guid userToken)
        {
            return _cache.TryGetValue(userToken.ToString(), out _);
        }
    }
}
