namespace BachBetV2.Domain.Models.Requests
{
    public sealed record PostNotificationRequest
    {
        public string UserId { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
    }
}
