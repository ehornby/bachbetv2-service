using BachBetV2.Application.DTOs;
using BachBetV2.Application.Enums;
using BachBetV2.Application.Extensions;
using BachBetV2.Application.Interfaces;
using BachBetV2.Application.Models;
using BachBetV2.Application.Models.Results;

namespace BachBetV2.Application.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILedgerRepository _ledgerRepository;
        private readonly IPushNotificationService _pushNotificationService;

        public UserService(
            IUserRepository userRepository,
            ILedgerRepository ledgerRepository,
            IPushNotificationService pushNotificationService)
        {
            _userRepository = userRepository;
            _ledgerRepository = ledgerRepository;
            _pushNotificationService = pushNotificationService;

        }

        public async Task<Result<List<User>>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            try
            {
                var userData = await _userRepository.GetAllUsers(cancellationToken);

                if (userData is null)
                {
                    return new NotFoundResult<List<User>>("No users retrieved");
                }

                var users = userData.Select(u => new User
                {
                    UserId = u.UserId.ToString(),
                    UserName = u.UserName,
                }).ToList();

                foreach (var user in users)
                {
                    user.Dbux = await _ledgerRepository.GetUserBalance(user.UserId, cancellationToken);
                }

                SuccessResult<List<User>> result = new(users);

                return result;
            }
            catch (Exception ex)
            {
                return ex.HandleError<List<User>>();
            }
        }

        public async Task<Result<List<Transaction>>> GetTransactionsByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            try
            {
                var transactionData = await _ledgerRepository.GetTransactionsByUserId(userId, cancellationToken);

                if (transactionData is null)
                {
                    return new NotFoundResult<List<Transaction>>("No transactions available for the specified user");
                }

                var transactions = transactionData.Select(t => new Transaction()
                {
                    Id = t.TransactionId,
                    Amount = t.Amount,
                    BetId = t.BetId,
                    ChallengeId = t.ChallengeId,
                    TransferUserId = t.TransferUserId,
                    TransferMessage = t.TransferMessage,
                    TransactionType = t.TransactionType,
                    Timestamp = t.Timestamp,
                }).ToList();

                return new SuccessResult<List<Transaction>>(transactions);
            }
            catch (Exception ex)
            {
                return ex.HandleError<List<Transaction>>();
            }
        }

        public async Task<Result<List<Transaction>>> GetAllTransactionsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var transactionData = await _ledgerRepository.GetAllTransactions(cancellationToken);

                if (transactionData is null)
                {
                    return new NotFoundResult<List<Transaction>>("No transactions available");
                }

                var transactions = transactionData.Select(t => new Transaction()
                {
                    Id = t.TransactionId,
                    Amount = t.Amount,
                    BetId = t.BetId,
                    ChallengeId = t.ChallengeId,
                    TransferUserId = t.TransferUserId,
                    TransferMessage = t.TransferMessage,
                    TransactionType = t.TransactionType,
                    Timestamp = t.Timestamp,
                }).ToList();

                return new SuccessResult<List<Transaction>>(transactions);
            }
            catch (Exception ex)
            {
                return ex.HandleError<List<Transaction>>();
            }
        }


        public async Task<Result<decimal>> BalanceTransferAsync(BalanceTransfer balanceTransfer, CancellationToken cancellationToken)
        {
            try
            {
                var senderBalance = await _ledgerRepository.GetUserBalance(balanceTransfer.SenderId, cancellationToken);

                if (senderBalance < balanceTransfer.Amount)
                {
                    return new ErrorResult<decimal>("too poor son");
                }

                BalanceTransferDto dto = new()
                {
                    Amount = balanceTransfer.Amount,
                    Message = balanceTransfer.Message,
                    ReceiverId = balanceTransfer.ReceiverId,
                    SenderId = balanceTransfer.SenderId,
                };

                await _userRepository.TransferBalance(dto, cancellationToken);

                var sender = await _userRepository.GetUserById(balanceTransfer.SenderId, cancellationToken);

                PushNotification notification = new()
                {
                    UserId = balanceTransfer.ReceiverId.ToString(),
                    Amount = balanceTransfer.Amount.ToString(),
                    NotificationType = NotificationType.Transfer,
                    RelatedUserName = sender!.UserName
                };

                await _pushNotificationService.SendPushNotificationAsync(notification, cancellationToken);

                var newBalance = senderBalance - balanceTransfer.Amount;

                return new SuccessResult<decimal>(newBalance);
            }
            catch (Exception ex)
            {
                return ex.HandleError<decimal>();
            }
        }
    }
}
