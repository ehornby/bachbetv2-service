using BachBetV2.Application.DTOs;
using BachBetV2.Application.Enums;
using BachBetV2.Application.Interfaces;
using BachBetV2.Application.Models;
using BachBetV2.Domain.Enums;
using BachBetV2.Infrastructure.Database.Contexts;
using BachBetV2.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BachBetV2.Infrastructure.Repositories
{
    public class BetRepository : IBetRepository
    {
        private readonly BachBetContext _context;

        public BetRepository(BachBetContext context)
        {
            _context = context;
        }

        public async Task<List<BetDto>> GetAllBets(CancellationToken cancellationToken)
        {
            var result = await _context.Bets.Select(b => new BetDto()
            {
                BetId = b.BetId,
                Description = b.Description,
                Odds = b.Odds,
                UserId = b.UserId.ToString(),
                Status = b.Status,
                Result = b.Result,
                Takers = b.Takers.Select(u => new Taker()
                {
                    UserId = u.Id.ToString(),
                    UserName = u.UserName
                }).ToList(),
                Tags = b.Tags.Select(t => new TagDto()
                {
                    TagId = t.Id.ToString(),
                    Description = t.TagDescription,
                }).ToList()
            }).ToListAsync(cancellationToken);

            foreach (var dto in result)
            {
                foreach (var taker in dto.Takers)
                {
                    taker.Wager = await _context.Ledger
                        .Where(t => t.BetId == dto.BetId && t.UserId == int.Parse(taker.UserId))
                        .Select(t => t.Amount)
                        .FirstOrDefaultAsync(cancellationToken);
                }
            }

            return result;
        }

        public async Task<BetDto?> GetBetById(int betId, CancellationToken cancellationToken)
        {
            var betEntity = await _context.Bets.FirstOrDefaultAsync(b => b.BetId == betId, cancellationToken);

            if (betEntity is not null)
            {
                return new()
                {
                    BetId = betEntity.BetId,
                    UserId = betEntity.UserId.ToString(),
                    Description = betEntity.Description,
                    Takers = betEntity.Takers.Select(t => new Taker()
                    {
                        UserId = t.Id.ToString(),
                        UserName = t.UserName
                    }).ToList()
                };
            }

            return null;
        }

        public async Task<int> CreateBet(BetDto dto, CancellationToken cancellationToken)
        {
            using var dbTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            BetEntity betEntity = new()
            {
                Description = dto.Description,
                Odds = dto.Odds,
                UserId = int.Parse(dto.UserId!),
                Status = BetStatus.Open,
            };

            if (dto.Tags.Count > 0)
            {
                foreach (var tag in dto.Tags)
                {
                    var tagEntity = await _context.Tags.FirstOrDefaultAsync(t => t.Id == int.Parse(tag.TagId!), cancellationToken);
                    tagEntity!.Bets.Add(betEntity);
                }
            }
            else
            {
                _context.Add(betEntity);
            }

            var result = await _context.SaveChangesAsync(cancellationToken);
            await dbTransaction.CommitAsync(cancellationToken);

            return result;
        }

        public async Task<BetDto?> TakeBet(TakeBetDto dto, CancellationToken cancellationToken)
        {
            using var dbTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            LedgerEntity entity = new()
            {
                UserId = int.Parse(dto.UserId!),
                Amount = dto.Wager,
                TransactionType = dto.TransactionType,
                BetId = int.Parse(dto.BetId!),
                Timestamp = DateTime.UtcNow,
            };

            _context.Add(entity);

            var betToTake = await _context.Bets
                .FirstOrDefaultAsync(
                    b => b.BetId == int.Parse(dto.BetId!)
                    && b.Status == BetStatus.Open
                    && b.UserId != int.Parse(dto.UserId!)
                    , cancellationToken);


            if (betToTake is null)
            {
                await dbTransaction.RollbackAsync(cancellationToken);
                return null;
            }

            LedgerEntity creatorEntity = new()
            {
                UserId = betToTake.UserId!,
                Amount = dto.Wager,
                TransactionType = TransactionType.WagerReceived,
                BetId = int.Parse(dto.BetId!),
                Timestamp = DateTime.UtcNow
            };

            _context.Add(creatorEntity);

            var taker = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == int.Parse(dto.UserId!), cancellationToken);

            betToTake?.Takers?.Add(taker!);
            _context.Update(betToTake!);

            await _context.SaveChangesAsync(cancellationToken);
            await dbTransaction.CommitAsync(cancellationToken);

            return new()
            {
                BetId = betToTake!.BetId,
                Description = betToTake.Description,
                UserId = betToTake.UserId.ToString(),
            };
        }

        public async Task<int?> CloseBet(int betId, CancellationToken cancellationToken)
        {
            var betToClose = await _context.Bets
                .Include(b => b.Takers)
                .FirstOrDefaultAsync(b => b.BetId == betId && b.Status == BetStatus.Open, cancellationToken);

            if (betToClose is not null)
            {
                betToClose.Status = BetStatus.Closed;

                return await _context.SaveChangesAsync(cancellationToken);
            }

            return null;
        }

        public async Task PayoutBet(PayoutBetDto dto, CancellationToken cancellationToken)
        {
            using var dbTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            var betId = int.Parse(dto.BetId);

            var betToPayout = await _context.Bets
                .Include(b => b.Takers)
                .FirstOrDefaultAsync(b => b.BetId == betId && b.Status == BetStatus.Closed, cancellationToken);

            if (betToPayout is not null)
            {
                betToPayout.Status = BetStatus.Paid;
                betToPayout.Result = dto.BetResult;

                await _context.SaveChangesAsync(cancellationToken);
            }

            var takerIds = betToPayout!.Takers.Select(t => t.Id).ToList();

            // If it didn't happen, all the takers lose - if it did, all the takers win
            foreach (var id in takerIds ?? new())
            {
                var wagerAmount = await _context.Ledger
                    .Where(
                        t => t.UserId == id &&
                        t.BetId == betId &&
                        t.TransactionType == TransactionType.Wager)
                    .Select(t => t.Amount)
                    .FirstOrDefaultAsync(cancellationToken);

                // Only additional losses if the bet creator loses
                if (dto.BetResult.Happened())
                {
                    LedgerEntity lossEntity = new()
                    {
                        Amount = wagerAmount * betToPayout!.Odds,
                        BetId = betId,
                        TransactionType = TransactionType.Loss,
                        UserId = betToPayout!.UserId,
                        Timestamp = DateTime.UtcNow
                    };

                    _context.Add(lossEntity);

                    LedgerEntity winEntity = new()
                    {
                        Amount = !dto.BetResult.Happened() ? wagerAmount : wagerAmount * betToPayout!.Odds,
                        BetId = betId,
                        TransactionType = TransactionType.Payout,
                        UserId = !dto.BetResult.Happened() ? betToPayout!.UserId : id,
                        Timestamp = DateTime.UtcNow
                    };

                    _context.Add(winEntity);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
            await dbTransaction.CommitAsync(cancellationToken);
        }
    }
}
