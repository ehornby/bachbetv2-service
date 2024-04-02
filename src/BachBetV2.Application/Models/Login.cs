namespace BachBetV2.Application.Models
{
    public sealed record Login
    {
        public string UserName { get; init; } = string.Empty;
        public string UserId { get; init; } = string.Empty;

        public Guid Token { get; init; }

    }
}
