using BachBetV2.Application.Models;
using BachBetV2.Application.Models.Results;

namespace BachBetV2.Application.Interfaces
{
    public interface IChallengeService
    {
        Task<Result<List<Challenge>>> GetAllChallengesAsync(CancellationToken cancellationToken);
        Task<Result> CreateChallengeAsync(Challenge challenge, CancellationToken cancellationToken);
        Task<Result<List<ChallengeClaim>?>> GetAllClaims(CancellationToken cancellationToken);
        Task<Result> ClaimChallengeAsync(ChallengeClaim claim, CancellationToken cancellationToken);
        Task<Result> WitnessClaimAsync(WitnessClaim witness, CancellationToken cancellationToken);
    }
}