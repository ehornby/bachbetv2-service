using BachBetV2.Domain.Enums;

namespace BachBetV2.Application.DTOs
{
    public sealed class PayoutBetDto
    {
        public string BetId { get; set; } = string.Empty;
        public BetResult BetResult { get; set; }
    }
}
