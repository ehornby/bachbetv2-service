using BachBetV2.Domain.Enums;

namespace BachBetV2.Application.Models
{
    public sealed class ChallengeClaim
    {
        public string? ClaimId { get; set; }
        public string ChallengeId { get; set; } = string.Empty;
        public ClaimStatus Status { get; set; }
        public string ClaimantId { get; set; } = string.Empty;
        public string? WitnessId { get; set; } = string.Empty;
        public DateTime? Timestamp { get; set; }
    }
}
