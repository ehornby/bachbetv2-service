namespace BachBetV2.Domain.Models.Requests
{
    public sealed class TakeBetRequest
    {
        public string? TakerId { get; init; }
        public decimal Wager { get; init; }
    }
}
