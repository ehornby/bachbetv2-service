namespace BachBetV2.Application.DTOs
{
    public sealed class ChallengeDto
    {
        public string ChallengeId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Reward { get; set; }
        public List<ChallengeClaimDto> Claims { get; set; } = new();
        public List<TagDto> Tags { get; set; } = new();
        public bool IsRepeatable { get; set; }
    }
}
