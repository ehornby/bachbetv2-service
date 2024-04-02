using BachBetV2.Application.DTOs;
using BachBetV2.Application.Enums;
using BachBetV2.Application.Interfaces;
using BachBetV2.Infrastructure.Database.Contexts;
using BachBetV2.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BachBetV2.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BachBetContext _context;

        public UserRepository(BachBetContext context)
        {
            _context = context;
        }

        public async Task<UserDto?> GetUserByUsername(string userName, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.UserName.ToLower() == userName.ToLower(), cancellationToken);

            if (user is not null)
            {
                return new()
                {
                    UserId = user.Id.ToString(),
                    UserName = user.UserName,
                    Token = user.Token,
                    Password = user.Password,
                };
            }

            return null;
        }

        public async Task<UserDto?> GetUserById(string userId, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(userId), cancellationToken);

            if (user is not null)
            {
                return new()
                {
                    UserId = user.Id.ToString(),
                    UserName = user.UserName,
                    Token = user.Token,
                };
            }

            return null;
        }

        public async Task<int?> GetUserId(string userName, CancellationToken cancellationToken)
        {
            int? userId = await _context.Users.Where(u => u.UserName == userName).Select(u => u.Id).FirstOrDefaultAsync(cancellationToken);

            if (userId is not null)
            {
                return userId;
            }

            return null;
        }

        public async Task<List<UserDto>?> GetAllUsers(CancellationToken cancellationToken)
        {
            var users = await _context.Users.Select(u => new UserDto()
            {
                UserId = u.Id.ToString(),
                UserName = u.UserName,
                Token = u.Token
            }).ToListAsync(cancellationToken);

            if (users is not null && users.Count > 0)
            {
                return users;
            }

            return null;
        }

        public async Task TransferBalance(BalanceTransferDto dto, CancellationToken cancellationToken)
        {
            using var dbTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            LedgerEntity withdrawalEntity = new()
            {
                TransactionType = TransactionType.TransferOut,
                Amount = dto.Amount,
                UserId = int.Parse(dto.SenderId),
                TransferUserId = int.Parse(dto.ReceiverId),
                Timestamp = DateTime.UtcNow,
                TransferMessage = dto.Message
            };

            LedgerEntity depositEntity = new()
            {
                TransactionType = TransactionType.TransferIn,
                Amount = dto.Amount,
                UserId = int.Parse(dto.ReceiverId),
                TransferUserId = int.Parse(dto.SenderId),
                Timestamp = DateTime.UtcNow,
                TransferMessage = dto.Message
            };

            _context.AddRange(withdrawalEntity, depositEntity);

            await _context.SaveChangesAsync(cancellationToken);
            await dbTransaction.CommitAsync(cancellationToken);
        }
    }
}