using BachBetV2.Application.DTOs;

namespace BachBetV2.Application.Interfaces
{
    public interface ILedgerRepository
    {
        Task<decimal> GetUserBalance(string userId, CancellationToken cancellationToken);
        Task<List<TransactionDto>> GetTransactionsByUserId(string userId, CancellationToken cancellationToken);
        Task<List<TransactionDto>> GetAllTransactions(CancellationToken cancellationToken);
        Task<TransactionDto?> GetWagerByBetId(string betId, CancellationToken cancellationToken);
    }
}