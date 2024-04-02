using BachBetV2.Application.Models;
using BachBetV2.Application.Models.Results;

namespace BachBetV2.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<Login>> ValidateUser(string userName, string password, CancellationToken cancellationToken);
    }
}