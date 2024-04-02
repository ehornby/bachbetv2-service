using BachBetV2.Domain.Enums;

namespace BachBetV2.Application.Models
{
    public sealed record PayoutBet
    {
        public string BetId { get; set; } = string.Empty;
        public BetResult BetResult { get; set; }
    }
}
