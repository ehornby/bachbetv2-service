using BachBetV2.Application.Extensions;
using BachBetV2.Application.Interfaces;
using BachBetV2.Application.Models;
using BachBetV2.Application.Models.Results;
using BachBetV2.Domain.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BachBetV2.WebApi.V1.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;

        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody, BindRequired] LoginRequest request, CancellationToken cancellationToken = default)
        {
            Result<Login> loginResult = await _authService.ValidateUser(request.UserName, request.Password, cancellationToken);

            return loginResult switch
            {
                SuccessResult<Login> successResult => new OkObjectResult(successResult.Data),
                UnauthorizedResult<Login> unauthorizedResult => new UnauthorizedObjectResult(unauthorizedResult.Message),
                ErrorResult<Login> errorResult => errorResult.GenerateErrorResponse(),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }
    }
}
