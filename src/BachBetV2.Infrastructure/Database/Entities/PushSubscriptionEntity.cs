using System.ComponentModel.DataAnnotations;

namespace BachBetV2.Infrastructure.Database.Entities
{
    public class PushSubscriptionEntity
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserEntity? User { get; set; }
        public string? Auth { get; set; }
        public string? P256DH { get; set; }
        public string? PushEndpoint { get; set; }
        public string? PublicKey { get; set; }
        public string? PrivateKey { get; set; }
    }
}