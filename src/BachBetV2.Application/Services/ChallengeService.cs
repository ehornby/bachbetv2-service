using BachBetV2.Application.DTOs;
using BachBetV2.Application.Enums;
using BachBetV2.Application.Extensions;
using BachBetV2.Application.Interfaces;
using BachBetV2.Application.Models;
using BachBetV2.Application.Models.Results;
using BachBetV2.Domain.Entities;

namespace BachBetV2.Application.Services
{
    public sealed class ChallengeService : IChallengeService
    {
        private readonly IChallengeRepository _challengeRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICachingService _cachingService;
        private readonly IPushNotificationService _pushNotificationService;

        public ChallengeService(
            IChallengeRepository challengeRepository,
            IUserRepository userRepository,
            ICachingService cachingService,
            IPushNotificationService pushNotificationService)
        {
            _challengeRepository = challengeRepository;
            _userRepository = userRepository;
            _cachingService = cachingService;
            _pushNotificationService = pushNotificationService;
        }

        public async Task<Result<List<Challenge>>> GetAllChallengesAsync(CancellationToken cancellationToken)
        {
            try
            {
                List<Challenge>? challenges = new();

                if (_cachingService.CacheIsCurrent<Challenge>() && _cachingService.CacheIsCurrent<ChallengeClaim>())
                {
                    challenges = _cachingService.GetCachedValue<Challenge>();

                    return new SuccessResult<List<Challenge>>(challenges!);
                }

                var dto = await _challengeRepository.GetAllChallenges(cancellationToken);

                if (dto is null)
                {
                    return new ErrorResult<List<Challenge>>("An error has occurred retrieving challenges.");
                }


                foreach (var challenge in dto)
                {
                    challenges.Add(new()
                    {
                        ChallengeId = challenge.ChallengeId,
                        Description = challenge.Description,
                        Reward = challenge.Reward,
                        Claims = challenge.Claims?.Select(c => new ChallengeClaim()
                        {
                            ClaimId = c.ClaimId,
                            ChallengeId = c.ChallengeId,
                            ClaimantId = c.ClaimantId,
                            WitnessId = c.WitnessId,
                            Timestamp = c.Timestamp,
                            Status = c.Status
                        }).ToList(),
                        Tags = challenge.Tags.Select(t => new Tag()
                        {
                            Id = t.TagId!,
                            Description = t.Description
                        }).ToList()
                    });
                }

                _cachingService.UpdateCache(challenges);

                return new SuccessResult<List<Challenge>>(challenges);
            }
            catch (Exception ex)
            {
                return ex.HandleError<List<Challenge>>();
            }
        }

        public async Task<Result> CreateChallengeAsync(Challenge challenge, CancellationToken cancellationToken)
        {
            try
            {
                ChallengeDto dto = new()
                {
                    Description = challenge.Description,
                    Reward = challenge.Reward,
                    Tags = challenge.Tags?.Select(t => new TagDto()
                    {
                        TagId = t.Id
                    }).ToList() ?? new(),
                    IsRepeatable = challenge.IsRepeatable,
                };

                await _challengeRepository.AddChallenge(dto, cancellationToken);
                _cachingService.SetCacheStatus<Challenge>(false);

                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return ex.HandleError();
            }
        }

        public async Task<Result<List<ChallengeClaim>?>> GetAllClaims(CancellationToken cancellationToken)
        {
            try
            {
                List<ChallengeClaim>? claims;

                if (_cachingService.CacheIsCurrent<ChallengeClaim>())
                {
                    claims = _cachingService.GetCachedValue<ChallengeClaim>();

                    return new SuccessResult<List<ChallengeClaim>?>(claims);
                }

                var dto = await _challengeRepository.GetAllClaims(cancellationToken);

                claims = dto?.Select(c => new ChallengeClaim()
                {
                    ClaimId = c.ClaimId,
                    ChallengeId = c.ChallengeId,
                    ClaimantId = c.ClaimantId,
                    WitnessId = c.WitnessId,
                    Timestamp = c.Timestamp,
                }).ToList()!;

                _cachingService.UpdateCache(claims);

                return new SuccessResult<List<ChallengeClaim>?>(claims);
            }
            catch (Exception ex)
            {
                return ex.HandleError<List<ChallengeClaim>?>();
            }
        }

        public async Task<Result> ClaimChallengeAsync(ChallengeClaim claim, CancellationToken cancellationToken)
        {
            try
            {
                ChallengeClaimDto dto = new()
                {
                    ChallengeId = claim.ChallengeId,
                    ClaimantId = claim.ClaimantId,
                };

                var result = await _challengeRepository.ClaimChallenge(dto, cancellationToken);

                if (result is null)
                {
                    return new ErrorResult("can't claim the same challenge twice you toilet");
                }

                _cachingService.SetCacheStatus<ChallengeClaim>(false);

                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return ex.HandleError();
            }
        }

        public async Task<Result> WitnessClaimAsync(WitnessClaim witness, CancellationToken cancellationToken)
        {
            try
            {
                var claim = await _challengeRepository.GetChallengeClaim(witness.ClaimId, cancellationToken);

                if (claim.ClaimantId == witness.WitnessId)
                {
                    return new ErrorResult("you can't witness your own challenge you swine");
                }

                WitnessClaimDto dto = new()
                {
                    ClaimId = witness.ClaimId,
                    WitnessId = witness.WitnessId,
                };

                var result = await _challengeRepository.WitnessChallengeClaim(dto, cancellationToken);

                if (result is null)
                {
                    return new ErrorResult("Claims can only be witnessed once.");
                }

                _cachingService.SetCacheStatus<ChallengeClaim>(false);

                var witnessUser = await _userRepository.GetUserById(witness.WitnessId!, cancellationToken);
                var challenge = await _challengeRepository.GetChallengeById(claim.ChallengeId, cancellationToken);

                PushNotification notification = new()
                {
                    UserId = claim.ClaimantId,
                    RelatedUserName = witnessUser!.UserName,
                    EntityId = claim.ClaimId,
                    EntityName = challenge!.Description,
                    NotificationType = NotificationType.ClaimWitnessed
                };

                await _pushNotificationService.SendPushNotificationAsync(notification, cancellationToken);

                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return ex.HandleError();
            }
        }
    }
}
