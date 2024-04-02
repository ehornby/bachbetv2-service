namespace BachBetV2.Domain.Models.Requests
{
    public sealed record LoginRequest
    {
        public string UserName { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}