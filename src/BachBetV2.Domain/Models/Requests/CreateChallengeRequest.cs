using BachBetV2.Domain.Entities;

namespace BachBetV2.Domain.Models.Requests
{
    public class CreateChallengeRequest
    {
        public string Description { get; init; } = string.Empty;
        public decimal Reward { get; init; }
        public List<Tag>? Tags { get; init; }
        public bool IsRepeatable { get; init; }
    }
}
