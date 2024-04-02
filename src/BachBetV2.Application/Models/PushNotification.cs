using BachBetV2.Application.Enums;

namespace BachBetV2.Application.Models
{
    public sealed class PushNotification
    {
        public string UserId { get; set; } = string.Empty;
        public string? RelatedUserName { get; set; }
        public string? RelatedUserId { get; set; }
        public string? EntityId { get; set; }
        public string? EntityName { get; set; }
        public string? Amount { get; set; }
        public string? Message { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
