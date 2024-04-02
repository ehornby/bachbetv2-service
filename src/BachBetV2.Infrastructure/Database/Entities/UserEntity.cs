using System.ComponentModel.DataAnnotations;

namespace BachBetV2.Infrastructure.Database.Entities
{
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }
        public Guid Token { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<BetEntity> Bets { get; set; } = new();
        public List<ChallengeClaimEntity> ChallengeClaims { get; set; } = new();
        public List<ChallengeClaimEntity> ChallengeWitnesses { get; set; } = new();
        public PushSubscriptionEntity? PushSubscription { get; set; }
    }
}
