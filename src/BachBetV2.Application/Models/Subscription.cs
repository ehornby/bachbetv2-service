namespace BachBetV2.Application.Models
{
    public sealed class Subscription
    {
        public string UserId { get; set; } = string.Empty;
        public string? Auth { get; set; }
        public string? P256DH { get; set; }
        public string? PushEndpoint { get; set; }
        public string? PublicKey { get; set; }
        public string? PrivateKey { get; set; }
    }
}
