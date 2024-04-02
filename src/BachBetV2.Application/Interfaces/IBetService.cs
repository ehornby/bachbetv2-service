using BachBetV2.Application.Models;
using BachBetV2.Application.Models.Results;

namespace BachBetV2.Application.Interfaces
{
    public interface IBetService
    {
        Task<Result<List<Bet>>> GetAllBetsAsync(CancellationToken cancellationToken);
        Task<Result> CreateBetAsync(Bet bet, CancellationToken cancellationToken);
        Task<Result> TakeBetAsync(TakeBet takeBet, CancellationToken cancellationToken);
        Task<Result> CloseBetAsync(int betId, CancellationToken cancellationToken);
        Task<Result> PayoutBetAsync(PayoutBet payoutBet, CancellationToken cancellationToken);
    }
}