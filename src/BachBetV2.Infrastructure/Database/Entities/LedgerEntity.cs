using BachBetV2.Application.Enums;
using System.ComponentModel.DataAnnotations;

namespace BachBetV2.Infrastructure.Database.Entities
{
    public class LedgerEntity
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public int? BetId { get; set; }
        public int? ChallengeId { get; set; }
        public int? TransferUserId { get; set; }
        public string? TransferMessage { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
