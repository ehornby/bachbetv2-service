namespace BachBetV2.Application.Models
{
    public sealed record Taker
    {
        public string UserId { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public decimal Wager { get; set; }
    }
}
