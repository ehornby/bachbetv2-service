using BachBetV2.Application.Models;
using BachBetV2.Application.Models.Results;

namespace BachBetV2.Application.Interfaces
{
    public interface IUserService
    {
        Task<Result<List<User>>> GetAllUsersAsync(CancellationToken cancellationToken);
        Task<Result<List<Transaction>>> GetTransactionsByUserIdAsync(string userId, CancellationToken cancellationToken);
        Task<Result<List<Transaction>>> GetAllTransactionsAsync(CancellationToken cancellationToken);
        Task<Result<decimal>> BalanceTransferAsync(BalanceTransfer balanceTransfer, CancellationToken cancellationToken);
    }
}