namespace BachBetV2.Application.Models
{
    public sealed class BalanceTransfer
    {
        public string SenderId { get; set; } = string.Empty;
        public string ReceiverId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? Message { get; set; }
    }
}
