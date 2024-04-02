using BachBetV2.Application.DTOs;
using BachBetV2.Application.Enums;
using BachBetV2.Application.Extensions;
using BachBetV2.Application.Interfaces;
using BachBetV2.Application.Models;
using BachBetV2.Application.Models.Results;
using BachBetV2.Domain.Entities;
using BachBetV2.Domain.Enums;

namespace BachBetV2.Application.Services
{
    public sealed class BetService : IBetService
    {
        private readonly IBetRepository _betRepository;
        private readonly ILedgerRepository _ledgerRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICachingService _cachingService;
        private readonly IPushNotificationService _notificationService;


        public BetService(
            IBetRepository betRepository,
            ILedgerRepository ledgerRepository,
            IUserRepository userRepository,
            ICachingService cachingService,
            IPushNotificationService notificationService)
        {
            _betRepository = betRepository;
            _ledgerRepository = ledgerRepository;
            _userRepository = userRepository;
            _cachingService = cachingService;
            _notificationService = notificationService;
        }

        public async Task<Result<List<Bet>>> GetAllBetsAsync(CancellationToken cancellationToken)
        {
            try
            {
                List<Bet>? bets = new();

                if (_cachingService.CacheIsCurrent<Bet>())
                {
                    bets = _cachingService.GetCachedValue<Bet>();

                    return new SuccessResult<List<Bet>>(bets!);
                }

                var dto = await _betRepository.GetAllBets(cancellationToken);

                if (dto is null)
                {
                    return new ErrorResult<List<Bet>>("An error has occurred retrieving bets");
                }

                foreach (var bet in dto)
                {
                    bets.Add(new()
                    {
                        Id = bet.BetId.ToString(),
                        Description = bet.Description,
                        Odds = bet.Odds,
                        Status = bet.Status,
                        Result = bet.Result,
                        Creator = new()
                        {
                            UserId = bet.UserId!,
                            Dbux = await _ledgerRepository.GetUserBalance(bet.UserId!, cancellationToken)
                        },
                        Takers = bet.Takers ?? new(),
                        Tags = bet.Tags.Select(t => new Tag()
                        {
                            Id = t.TagId!,
                            Description = t.Description
                        }).ToList()
                    });
                }

                _cachingService.UpdateCache(bets);

                return new SuccessResult<List<Bet>>(bets);
            }
            catch (Exception ex)
            {
                return ex.HandleError<List<Bet>>();
            }
        }

        public async Task<Result> CreateBetAsync(Bet bet, CancellationToken cancellationToken)
        {
            try
            {
                var balance = await _ledgerRepository.GetUserBalance(bet.Creator.UserId, cancellationToken);

                if (balance < 0)
                {
                    return new BadRequestErrorResult("can't take action when you're poor little son");
                }

                BetDto dto = new()
                {
                    Description = bet.Description,
                    Odds = bet.Odds,
                    UserId = bet.Creator.UserId,
                    Status = BetStatus.Open,
                    Tags = bet.Tags?.Select(t => new TagDto()
                    {
                        TagId = t.Id,
                        Description = t.Description
                    }).ToList() ?? new()
                };

                var createBetResult = await _betRepository.CreateBet(dto, cancellationToken);

                if (createBetResult > 0)
                {
                    _cachingService.SetCacheStatus<Bet>(false);
                    return new SuccessResult();
                }

                return new ErrorResult($"An error has occurred creating your bet.");
            }
            catch (Exception ex)
            {
                return ex.HandleError();
            }
        }

        public async Task<Result> TakeBetAsync(TakeBet takeBet, CancellationToken cancellationToken)
        {
            try
            {
                var userBalance = await _ledgerRepository.GetUserBalance(takeBet.UserId!, cancellationToken);

                if (userBalance < takeBet.Wager)
                {
                    return new BadRequestErrorResult("busto boiiiiiiiiiii get some stacks fish");
                }

                TakeBetDto betDto = new()
                {
                    UserId = takeBet.UserId,
                    UserToken = takeBet.UserToken,
                    BetId = takeBet.BetId,
                    Wager = takeBet.Wager,
                    TransactionType = TransactionType.Wager
                };

                var takenBet = await _betRepository.TakeBet(betDto, cancellationToken);

                if (takenBet is not null)
                {
                    _cachingService.SetCacheStatus<Bet>(false);

                    var relatedUser = await _userRepository.GetUserById(betDto.UserId, cancellationToken);

                    PushNotification notification = new()
                    {
                        NotificationType = NotificationType.BetTaken,
                        UserId = takenBet.UserId!,
                        Amount = betDto.Wager.ToString(),
                        RelatedUserName = relatedUser!.UserName,
                        EntityId = betDto.BetId,
                        EntityName = takenBet.Description
                    };

                    await _notificationService.SendPushNotificationAsync(notification, cancellationToken);

                    return new SuccessResult();
                }

                return new ErrorResult("Cannot take this bet");

            }
            catch (Exception ex)
            {
                return ex.HandleError();
            }
        }

        public async Task<Result> CloseBetAsync(int betId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _betRepository.CloseBet(betId, cancellationToken);

                if (result is not null)
                {
                    var closedBet = await _betRepository.GetBetById(betId, cancellationToken);
                    var betCreator = await _userRepository.GetUserById(closedBet!.UserId!, cancellationToken);

                    _cachingService.SetCacheStatus<Bet>(false);

                    foreach (var taker in closedBet!.Takers)
                    {
                        PushNotification notification = new()
                        {
                            NotificationType = NotificationType.BetClosed,
                            UserId = taker.UserId,
                            RelatedUserName = betCreator!.UserName,
                            EntityId = closedBet!.BetId.ToString(),
                            EntityName = closedBet.Description
                        };

                        await _notificationService.SendPushNotificationAsync(notification, cancellationToken);
                    }

                    return new SuccessResult();
                }

                return new ErrorResult("Cannot close this bet.");
            }
            catch (Exception ex)
            {
                return ex.HandleError();
            }
        }

        public async Task<Result> PayoutBetAsync(PayoutBet payoutBet, CancellationToken cancellationToken)
        {
            try
            {
                PayoutBetDto dto = new()
                {
                    BetId = payoutBet.BetId,
                    BetResult = payoutBet.BetResult
                };

                await _betRepository.PayoutBet(dto, cancellationToken);
                _cachingService.SetCacheStatus<Bet>(false);

                var paidBet = await _betRepository.GetBetById(int.Parse(payoutBet.BetId), cancellationToken);
                var betCreator = await _userRepository.GetUserById(paidBet!.UserId!, cancellationToken);
                var wager = await _ledgerRepository.GetWagerByBetId(paidBet.BetId.ToString(), cancellationToken);

                foreach (var taker in paidBet!.Takers)
                {
                    PushNotification notification = new()
                    {
                        NotificationType = payoutBet.BetResult == BetResult.DidNotHappen
                            ? NotificationType.BetPayoutLoss
                            : NotificationType.BetPayoutWin,
                        UserId = taker.UserId,
                        Amount = wager!.Amount!.ToString(),
                        RelatedUserName = betCreator!.UserName,
                        EntityId = paidBet!.BetId.ToString(),
                        EntityName = paidBet.Description
                    };

                    await _notificationService.SendPushNotificationAsync(notification, cancellationToken);
                }

                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return ex.HandleError();
            }
        }
    }
}