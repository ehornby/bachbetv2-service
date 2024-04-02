using BachBetV2.Application.Extensions;
using BachBetV2.Application.Interfaces;
using BachBetV2.Application.Models;
using BachBetV2.Application.Models.Results;
using BachBetV2.Domain.Models.Requests;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BachBetV2.WebApi.V1.Controllers
{
    [Route("v1")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken = default)
        {
            Result<List<User>> userResult = await _userService.GetAllUsersAsync(cancellationToken);

            return userResult switch
            {
                SuccessResult<List<User>> successResult => new OkObjectResult(successResult.Data),
                NotFoundResult<List<User>> notFoundResult => new NotFoundObjectResult(notFoundResult.Message),
                ErrorResult<List<User>> errorResult => errorResult.GenerateErrorResponse(),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }

        [HttpGet]
        [Route("[controller]({userId})/transactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTransactionsByUserId([FromRoute, BindRequired] int userId, CancellationToken cancellationToken = default)
        {
            Result<List<Transaction>> transactionResult = await _userService.GetTransactionsByUserIdAsync(userId.ToString(), cancellationToken);

            return transactionResult switch
            {
                SuccessResult<List<Transaction>> successResult => new OkObjectResult(successResult.Data),
                NotFoundResult<List<Transaction>> notFoundResult => new NotFoundObjectResult(notFoundResult.Message),
                ErrorResult<List<Transaction>> errorResult => errorResult.GenerateErrorResponse(),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }

        [HttpGet]
        [Route("[controller]/transactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllTransactions(CancellationToken cancellationToken = default)
        {
            Result<List<Transaction>> transactionResult = await _userService.GetAllTransactionsAsync(cancellationToken);

            return transactionResult switch
            {
                SuccessResult<List<Transaction>> successResult => new OkObjectResult(successResult.Data),
                NotFoundResult<List<Transaction>> notFoundResult => new NotFoundObjectResult(notFoundResult.Message),
                ErrorResult<List<Transaction>> errorResult => errorResult.GenerateErrorResponse(),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }


        [HttpPost]
        [Route("[controller]({userId})/transfer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostBalanceTransfer(
            [FromRoute, BindRequired] int userId,
            [FromBody, BindRequired] BalanceTransferRequest request,
            [FromServices] IValidator<BalanceTransferRequest> validator,
            CancellationToken cancellationToken = default)
        {
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors);
            }

            BalanceTransfer balanceTransfer = new()
            {
                SenderId = userId.ToString(),
                ReceiverId = request.ReceivingUserId,
                Amount = request.Amount,
                Message = request.Message,
            };

            Result transferResult = await _userService.BalanceTransferAsync(balanceTransfer, cancellationToken);

            return transferResult switch
            {
                SuccessResult<decimal> successResult => new OkObjectResult(successResult.Data),
                BadRequestErrorResult badRequestResult => new BadRequestObjectResult(badRequestResult.Message),
                ErrorResult<decimal> errorResult => errorResult.GenerateErrorResponse(),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }
    }
}
