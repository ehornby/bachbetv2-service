using BachBetV2.Application.DTOs;

namespace BachBetV2.Application.Interfaces
{
    public interface IChallengeRepository
    {
        Task<List<ChallengeDto>> GetAllChallenges(CancellationToken cancellationToken);
        Task<ChallengeDto?> GetChallengeById(string challengeId, CancellationToken cancellationToken);
        Task AddChallenge(ChallengeDto dto, CancellationToken cancellationToken);
        Task<List<ChallengeClaimDto>?> GetAllClaims(CancellationToken cancellationToken);
        Task<int?> ClaimChallenge(ChallengeClaimDto dto, CancellationToken cancellationToken);
        Task<int?> WitnessChallengeClaim(WitnessClaimDto dto, CancellationToken cancellationToken);
        Task<ChallengeClaimDto> GetChallengeClaim(string challengeClaimId, CancellationToken cancellationToken);
    }
}