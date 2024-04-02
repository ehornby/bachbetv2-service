using BachBetV2.Domain.Entities;
using BachBetV2.Domain.Enums;

namespace BachBetV2.Application.Models
{
    public sealed record Bet
    {
        public string Id { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Odds { get; set; }

        public BetStatus Status { get; set; }

        public BetResult? Result { get; set; }

        public User Creator { get; set; } = new User();

        public List<Taker>? Takers { get; set; }

        public List<Tag>? Tags { get; set; }
    }
}
