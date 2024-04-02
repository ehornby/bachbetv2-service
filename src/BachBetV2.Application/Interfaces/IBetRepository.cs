using BachBetV2.Application.DTOs;

namespace BachBetV2.Application.Interfaces
{
    public interface IBetRepository
    {
        Task<List<BetDto>> GetAllBets(CancellationToken cancellationToken);
        Task<BetDto?> GetBetById(int betId, CancellationToken cancellationToken);
        Task<int> CreateBet(BetDto dto, CancellationToken cancellationToken);
        Task<BetDto?> TakeBet(TakeBetDto dto, CancellationToken cancellationToken);
        Task<int?> CloseBet(int betId, CancellationToken cancellationToken);
        Task PayoutBet(PayoutBetDto dto, CancellationToken cancellationToken);
    }
}