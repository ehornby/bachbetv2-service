namespace BachBetV2.Domain.Models.Requests
{
    public sealed record CreateSubscriptionRequest
    {
        public string UserId { get; init; } = string.Empty;
        public string? Auth { get; init; }
        public string? P256DH { get; init; }
        public string? PushEndpoint { get; init; }
        public string? PublicKey { get; init; }
        public string? PrivateKey { get; init; }
    }
}
