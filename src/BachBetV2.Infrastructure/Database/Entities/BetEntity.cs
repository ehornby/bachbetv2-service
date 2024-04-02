using BachBetV2.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BachBetV2.Infrastructure.Database.Entities
{
    public class BetEntity
    {
        [Key]
        public int BetId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Odds { get; set; }
        public int UserId { get; set; }
        public BetStatus Status { get; set; }
        public BetResult? Result { get; set; }
        public List<UserEntity> Takers { get; set; } = new();
        public List<TagEntity> Tags { get; set; } = new();
    }
}
