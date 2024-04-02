namespace BachBetV2.Domain.Models.Requests
{
    public sealed class BalanceTransferRequest
    {
        public string ReceivingUserId { get; init; } = string.Empty;
        public decimal Amount { get; init; }
        public string? Message { get; init; }
    }
}
