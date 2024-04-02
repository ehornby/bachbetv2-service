using System.ComponentModel.DataAnnotations;

namespace BachBetV2.Infrastructure.Database.Entities
{
    public class ChallengeEntity
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Reward { get; set; }
        public List<TagEntity> Tags { get; set; } = new();
        public bool IsRepeatable { get; set; }
    }
}
