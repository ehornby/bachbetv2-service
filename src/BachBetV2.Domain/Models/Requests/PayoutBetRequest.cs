using BachBetV2.Domain.Enums;

namespace BachBetV2.Domain.Models.Requests
{
    public sealed class PayoutBetRequest
    {
        public BetResult Result { get; init; }
    }
}
