using BachBetV2.Application.DTOs;

namespace BachBetV2.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDto?> GetUserByUsername(string userName, CancellationToken cancellationToken);
        Task<UserDto?> GetUserById(string userId, CancellationToken cancellationToken);
        Task<int?> GetUserId(string userName, CancellationToken cancellationToken);
        Task<List<UserDto>?> GetAllUsers(CancellationToken cancellationToken);
        Task TransferBalance(BalanceTransferDto dto, CancellationToken cancellationToken);
    }
}