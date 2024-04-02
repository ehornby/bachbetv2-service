using BachBetV2.Application.Extensions;
using BachBetV2.Application.Interfaces;
using BachBetV2.Application.Models;
using BachBetV2.Application.Models.Results;
using BachBetV2.Domain.Enums;
using BachBetV2.Domain.Models.Requests;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BachBetV2.WebApi.V1.Controllers
{
    [Route("v1")]
    [ApiController]
    public class BetsController : ControllerBase
    {
        private readonly IBetService _betService;

        public BetsController(IBetService betService)
        {
            _betService = betService;
        }

        [HttpGet]
        [Route("[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetAllBets(CancellationToken cancellationToken = default)
        {
            return await _betService.GetAllBetsAsync(cancellationToken) switch
            {
                SuccessResult<List<Bet>> successResult => new OkObjectResult(successResult.Data),
                ErrorResult<List<Bet>> errorResult => errorResult.GenerateErrorResponse(),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }

        [HttpPost]
        [Route("[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostBet(
            [FromBody, BindRequired] CreateBetRequest request,
            [FromServices] IValidator<CreateBetRequest> validator,
            CancellationToken cancellationToken = default)
        {
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors);
            }

            Bet bet = new()
            {
                Description = request.Description,
                Odds = request.Odds,
                Status = BetStatus.Open,
                Creator = new()
                {
                    UserId = request.UserId,
                },
                Tags = request.Tags
            };

            return await _betService.CreateBetAsync(bet, cancellationToken) switch
            {
                SuccessResult => new OkResult(),
                BadRequestErrorResult badRequestResult => new BadRequestObjectResult(badRequestResult.Message),
                ErrorResult errorResult => errorResult.GenerateErrorResponse(),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }

        [HttpPost]
        [Route("[controller]({betId})/take")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostTakeBet(
            [FromRoute, BindRequired] string betId,
            [FromBody, BindRequired] TakeBetRequest request,
            [FromHeader, BindRequired] string userToken,
            [FromServices] IValidator<TakeBetRequest> validator,
            CancellationToken cancellationToken = default)
        {
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors);
            }

            TakeBet takeBet = new()
            {
                UserId = request.TakerId!,
                UserToken = Guid.Parse(userToken),
                BetId = betId,
                Wager = request.Wager
            };

            return await _betService.TakeBetAsync(takeBet, cancellationToken) switch
            {
                SuccessResult => new OkResult(),
                BadRequestErrorResult badRequestResult => new BadRequestObjectResult(badRequestResult.Message),
                ErrorResult errorResult => errorResult.GenerateErrorResponse(),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }

        [HttpPatch]
        [Route("[controller]({betId})/close")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchCloseBet([FromRoute, BindRequired] int betId, CancellationToken cancellationToken = default)
        {
            return await _betService.CloseBetAsync(betId, cancellationToken) switch
            {
                SuccessResult => new OkResult(),
                ErrorResult errorResult => errorResult.GenerateErrorResponse(),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }

        [HttpPost]
        [Route("[controller]({betId})/payout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostPayoutBet(
            [FromRoute, BindRequired] int betId,
            [FromBody, BindRequired] PayoutBetRequest request,
            CancellationToken cancellationToken = default)
        {
            PayoutBet payout = new()
            {
                BetId = betId.ToString(),
                BetResult = request.Result,
            };

            return await _betService.PayoutBetAsync(payout, cancellationToken) switch
            {
                SuccessResult => new OkResult(),
                ErrorResult errorResult => errorResult.GenerateErrorResponse(),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }
    }
}
