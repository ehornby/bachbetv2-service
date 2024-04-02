using BachBetV2.Application.Enums;

namespace BachBetV2.Application.DTOs
{
    public sealed class TakeBetDto
    {
        public string? UserId { get; set; }
        public Guid UserToken { get; set; }
        public string? BetId { get; set; }
        public decimal Wager { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
