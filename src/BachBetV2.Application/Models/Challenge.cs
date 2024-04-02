using BachBetV2.Domain.Entities;

namespace BachBetV2.Application.Models
{
    public class Challenge
    {
        public string ChallengeId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Reward { get; set; }
        public List<ChallengeClaim>? Claims { get; set; }
        public List<Tag>? Tags { get; set; }
        public bool IsRepeatable { get; set; }
    }
}
