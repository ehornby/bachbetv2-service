using BachBetV2.Application.Models;
using BachBetV2.Domain.Enums;

namespace BachBetV2.Application.DTOs
{
    public class BetDto
    {
        public int BetId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Odds { get; set; }
        public string? UserId { get; set; }
        public BetStatus Status { get; set; }
        public BetResult? Result { get; set; }
        public List<TagDto> Tags { get; set; } = new();
        public List<Taker> Takers { get; set; } = new();
    }
}
