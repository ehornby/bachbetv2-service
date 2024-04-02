using BachBetV2.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BachBetV2.Infrastructure.Database.Entities
{
    public class ChallengeClaimEntity
    {
        [Key]
        public int Id { get; set; }
        public UserEntity Claimaint { get; set; } = new();
        public ChallengeEntity Challenge { get; set; } = new();
        public ClaimStatus Status { get; set; }
        public UserEntity? Witness { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
