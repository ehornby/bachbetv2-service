using BachBetV2.Application.DTOs;
using BachBetV2.Application.Enums;
using BachBetV2.Application.Interfaces;
using BachBetV2.Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BachBetV2.Infrastructure.Repositories
{
    public sealed class LedgerRepository : ILedgerRepository
    {
        private readonly BachBetContext _context;

        public LedgerRepository(BachBetContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetUserBalance(string userId, CancellationToken cancellationToken)
        {
            var id = int.Parse(userId);

            var seed = await _context.Ledger
                .Where(t => t.UserId == id && t.TransactionType == TransactionType.Seed)
                .SumAsync(t => t.Amount, cancellationToken);

            var wagers = await _context.Ledger
                .Where(t => t.UserId == id && t.TransactionType == TransactionType.Wager)
                .SumAsync(t => t.Amount, cancellationToken);

            var payouts = await _context.Ledger
                .Where(t => t.UserId == id && t.TransactionType == TransactionType.Payout)
                .SumAsync(t => t.Amount, cancellationToken);

            var losses = await _context.Ledger
                .Where(t => t.UserId == id && t.TransactionType == TransactionType.Loss)
                .SumAsync(t => t.Amount, cancellationToken);

            var challenges = await _context.Ledger
                .Where(t => t.UserId == id && t.TransactionType == TransactionType.Challenge)
                .SumAsync(t => t.Amount, cancellationToken);

            var transfersIn = await _context.Ledger
                .Where(t => t.UserId == id && t.TransactionType == TransactionType.TransferIn)
                .SumAsync(t => t.Amount, cancellationToken);

            var transfersOut = await _context.Ledger
                .Where(t => t.UserId == id && t.TransactionType == TransactionType.TransferOut)
                .SumAsync(t => t.Amount, cancellationToken);

            var wagersReceived = await _context.Ledger
                .Where(t => t.UserId == id && t.TransactionType == TransactionType.WagerReceived)
                .SumAsync(t => t.Amount, cancellationToken);

            var balance =
                seed +
                payouts +
                challenges +
                wagersReceived +
                transfersIn -
                wagers -
                losses -
                transfersOut;

            return balance;
        }

        public async Task<List<TransactionDto>> GetTransactionsByUserId(string userId, CancellationToken cancellationToken)
        {
            var id = int.Parse(userId);

            List<TransactionDto> transactions = await _context.Ledger
                .Where(t => t.UserId == id)
                .Select(t => new TransactionDto()
                {
                    TransactionId = t.Id.ToString(),
                    Amount = t.Amount,
                    BetId = t.BetId.ToString(),
                    ChallengeId = t.ChallengeId.ToString(),
                    TransferUserId = t.TransferUserId.ToString(),
                    TransferMessage = t.TransferMessage,
                    TransactionType = t.TransactionType,
                    Timestamp = t.Timestamp,
                }).ToListAsync(cancellationToken);

            return transactions;
        }

        public async Task<List<TransactionDto>> GetAllTransactions(CancellationToken cancellationToken)
        {
            List<TransactionDto> transactions = await _context.Ledger
                .Select(t => new TransactionDto()
                {
                    TransactionId = t.Id.ToString(),
                    Amount = t.Amount,
                    BetId = t.BetId.ToString(),
                    ChallengeId = t.ChallengeId.ToString(),
                    TransferUserId = t.TransferUserId.ToString(),
                    TransferMessage = t.TransferMessage,
                    TransactionType = t.TransactionType,
                    Timestamp = t.Timestamp,
                }).ToListAsync(cancellationToken);

            return transactions;
        }

        public async Task<TransactionDto?> GetWagerByBetId(string betId, CancellationToken cancellationToken)
        {
            var wager = await _context.Ledger
                .Where(t => t.BetId == int.Parse(betId) && t.TransactionType == TransactionType.Wager)
                .Select(t => new TransactionDto()
                {
                    BetId = t.BetId.ToString(),
                    Amount = t.Amount,
                    TransactionType = t.TransactionType,
                })
                .FirstOrDefaultAsync(cancellationToken);

            return wager;
        }
    }
}
