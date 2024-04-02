using BachBetV2.Application.DTOs;
using BachBetV2.Application.Enums;
using BachBetV2.Application.Interfaces;
using BachBetV2.Domain.Enums;
using BachBetV2.Infrastructure.Database.Contexts;
using BachBetV2.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BachBetV2.Infrastructure.Repositories
{
    public sealed class ChallengeRepository : IChallengeRepository
    {
        private readonly BachBetContext _context;

        public ChallengeRepository(BachBetContext context)
        {
            _context = context;
        }

        public async Task<ChallengeClaimDto> GetChallengeClaim(string challengeClaimId, CancellationToken cancellationToken)
        {
            var claim = await _context.ChallengeClaims.Select(cc => new ChallengeClaimDto()
            {
                ChallengeId = cc.Challenge.Id.ToString(),
                ClaimantId = cc.Claimaint.Id.ToString(),
                ClaimId = cc.Id.ToString(),
                WitnessId = cc.Witness != null ? cc.Witness.Id.ToString() : null,
                Timestamp = cc.Timestamp,
                Status = cc.Status
            }).FirstOrDefaultAsync(x => x.ClaimId == challengeClaimId, cancellationToken);

            return claim!;
        }

        public async Task<List<ChallengeDto>> GetAllChallenges(CancellationToken cancellationToken)
        {
            var challenges = await _context.Challenges.Select(c => new ChallengeDto()
            {
                ChallengeId = c.Id.ToString(),
                Description = c.Description,
                Reward = c.Reward,
                Tags = c.Tags.Select(t => new TagDto()
                {
                    TagId = t.Id.ToString(),
                    Description = t.TagDescription,
                }).ToList()

            }).ToListAsync(cancellationToken);

            var claims = await _context.ChallengeClaims.Select(cc => new ChallengeClaimDto()
            {
                ChallengeId = cc.Challenge.Id.ToString(),
                ClaimantId = cc.Claimaint.Id.ToString(),
                ClaimId = cc.Id.ToString(),
                WitnessId = cc.Witness != null ? cc.Witness.Id.ToString() : null,
                Timestamp = cc.Timestamp,
                Status = cc.Status
            }).ToListAsync(cancellationToken);

            foreach (var challenge in challenges)
            {
                var challengeClaims = claims.Where(c => c.ChallengeId == challenge.ChallengeId).ToList();

                if (challengeClaims.Count > 0)
                {
                    challenge.Claims = challengeClaims;
                }
            }

            return challenges;
        }

        public async Task<ChallengeDto?> GetChallengeById(string challengeId, CancellationToken cancellationToken)
        {
            return await _context.Challenges.Where(c => c.Id == int.Parse(challengeId))
                .Select(c => new ChallengeDto()
                {
                    ChallengeId = c.Id.ToString(),
                    Description = c.Description,
                }).FirstOrDefaultAsync(cancellationToken);

        }

        public async Task AddChallenge(ChallengeDto dto, CancellationToken cancellationToken)
        {
            var tags = await _context.Tags.Where(x => dto.Tags.Select(x => x.TagId).Contains(x.Id.ToString())).ToListAsync();

            ChallengeEntity entity = new()
            {
                Description = dto.Description,
                Reward = dto.Reward,
                IsRepeatable = dto.IsRepeatable,
                Tags = tags,
            };

            _context.Challenges.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<ChallengeClaimDto>?> GetAllClaims(CancellationToken cancellationToken)
        {
            var claims = await _context.ChallengeClaims.Select(cc => new ChallengeClaimDto()
            {
                ClaimId = cc.Id.ToString(),
                ChallengeId = cc.Challenge.Id.ToString(),
                ClaimantId = cc.Claimaint.Id.ToString(),
                WitnessId = cc.Witness != null ? cc.Witness.Id.ToString() : null,
                Timestamp = cc.Timestamp
            }).ToListAsync(cancellationToken);

            return claims;
        }

        public async Task<int?> ClaimChallenge(ChallengeClaimDto dto, CancellationToken cancellationToken)
        {
            var challengeToClaim = await _context.Challenges
                .Where(c => c.Id == int.Parse(dto.ChallengeId))
                .FirstOrDefaultAsync(cancellationToken);

            var challengeId = challengeToClaim!.Id;

            if (!challengeToClaim.IsRepeatable)
            {
                var isAlreadyClaimed = await _context.ChallengeClaims.AnyAsync(c => c.Claimaint.Id == int.Parse(dto.ClaimantId) && c.Challenge.Id == challengeId, cancellationToken);

                if (isAlreadyClaimed)
                {
                    return null;
                }
            }

#pragma warning disable CS8601 // Possible null reference assignment.
            ChallengeClaimEntity entity = new()
            {
                Challenge = await _context.Challenges.FirstOrDefaultAsync(c => c.Id == challengeId, cancellationToken),
                Claimaint = await _context.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(dto.ClaimantId), cancellationToken),
                Timestamp = DateTime.UtcNow,
            };
#pragma warning restore CS8601 // Possible null reference assignment.

            _context.Add(entity);

            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int?> WitnessChallengeClaim(WitnessClaimDto dto, CancellationToken cancellationToken)
        {
            using var dbTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            var claim = await _context.ChallengeClaims
                .Where(c => c.Id == int.Parse(dto.ClaimId) && c.Status == ClaimStatus.NotWitnessed)
                .Include(c => c.Challenge)
                .Include(c => c.Claimaint)
                .FirstOrDefaultAsync(cancellationToken);

            if (claim is null)
            {
                await dbTransaction.RollbackAsync(cancellationToken);
                return null;
            }

            claim.Status = ClaimStatus.Witnessed;
            claim.Witness = await _context.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(dto.WitnessId), cancellationToken);

            LedgerEntity challengePayout = new()
            {
                ChallengeId = claim.Challenge.Id,
                Amount = claim.Challenge.Reward,
                TransactionType = TransactionType.Challenge,
                UserId = claim.Claimaint.Id,
                Timestamp = DateTime.UtcNow,
            };

            _context.Add(challengePayout);

            var result = await _context.SaveChangesAsync(cancellationToken);
            await dbTransaction.CommitAsync(cancellationToken);

            return result;
        }
    }
}
