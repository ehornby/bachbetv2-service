using BachBetV2.Application.Enums;

namespace BachBetV2.Application.Models
{
    public sealed class Transaction
    {
        public string Id { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public string? BetId { get; set; }
        public string? ChallengeId { get; set; }
        public string? TransferUserId { get; set; }
        public string? TransferMessage { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
