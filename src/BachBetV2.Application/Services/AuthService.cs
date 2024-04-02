using BachBetV2.Application.Extensions;
using BachBetV2.Application.Interfaces;
using BachBetV2.Application.Models;
using BachBetV2.Application.Models.Results;

namespace BachBetV2.Application.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<Login>> ValidateUser(string userName, string password, CancellationToken cancellationToken)
        {
            try
            {
                var userData = await _userRepository.GetUserByUsername(userName, cancellationToken);

                if (userData?.Password == password)
                {
                    return new SuccessResult<Login>(new()
                    {
                        UserName = userData.UserName,
                        UserId = userData.UserId.ToString(),
                        Token = userData.Token,
                    });
                }

                if (userData is null)
                {
                    return new UnauthorizedResult<Login>("Could not find user");
                }

                return new UnauthorizedResult<Login>("Incorrect password");
            }
            catch (Exception ex)
            {
                return ex.HandleError<Login>();
            }
        }
    }
}
