using System.ComponentModel.DataAnnotations;

namespace BachBetV2.Infrastructure.Database.Entities
{
    public class TagEntity
    {
        [Key]
        public int Id { get; set; }
        public string TagDescription { get; set; } = string.Empty;
        public List<ChallengeEntity> Challenges { get; set; } = new();
        public List<BetEntity> Bets { get; set; } = new();
    }
}
