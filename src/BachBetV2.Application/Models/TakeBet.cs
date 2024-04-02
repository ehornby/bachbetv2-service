namespace BachBetV2.Application.Models
{
    public sealed record TakeBet
    {
        public string UserId { get; set; } = string.Empty;
        public Guid UserToken { get; set; }
        public string? BetId { get; set; }
        public decimal Wager { get; set; }
    }
}
