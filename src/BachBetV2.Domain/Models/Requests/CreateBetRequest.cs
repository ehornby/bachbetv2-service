using BachBetV2.Domain.Entities;

namespace BachBetV2.Domain.Models.Requests
{
    public sealed record CreateBetRequest
    {
        public string Description { get; init; } = string.Empty;
        public decimal Odds { get; init; }
        public string UserId { get; init; } = string.Empty;
        public DateTime? Expiry { get; init; }
        public List<Tag>? Tags { get; init; }
    }
}
