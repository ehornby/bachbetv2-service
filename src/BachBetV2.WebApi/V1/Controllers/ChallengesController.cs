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
    public class ChallengesController : ControllerBase
    {
        private readonly IChallengeService _challengeService;

        public ChallengesController(IChallengeService challengeService)
        {
            _challengeService = challengeService;
        }

        [HttpGet]
        [Route("[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllChallenges(CancellationToken cancellationToken = default)
        {
            return await _challengeService.GetAllChallengesAsync(cancellationToken) switch
            {
                SuccessResult<List<Challenge>> successResult => new OkObjectResult(successResult.Data),
                ErrorResult<List<Challenge>> errorResult => errorResult.GenerateErrorResponse(),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }

        [HttpPost]
        [Route("[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostChallenge(
            [FromBody, BindRequired] CreateChallengeRequest request,
            [FromServices] IValidator<CreateChallengeRequest> validator,
            CancellationToken cancellationToken = default)
        {
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors);
            }

            Challenge challenge = new()
            {
                Description = request.Description,
                Reward = request.Reward,
                Tags = request.Tags,
                IsRepeatable = request.IsRepeatable,
            };

            return await _challengeService.CreateChallengeAsync(challenge, cancellationToken) switch
            {
                SuccessResult => new OkResult(),
                ErrorResult errorResult => errorResult.GenerateErrorResponse(),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }

        [HttpGet]
        [Route("[controller]/claims")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllClaims(CancellationToken cancellationToken = default)
        {
            return await _challengeService.GetAllClaims(cancellationToken) switch
            {
                SuccessResult<List<ChallengeClaim>?> successResult => new OkObjectResult(successResult.Data),
                ErrorResult<List<ChallengeClaim>?> errorResult => errorResult.GenerateErrorResponse(),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }


        [HttpPost]
        [Route("[controller]({challengeId})/claims")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostChallengeClaim(
            [FromBody, BindRequired] ChallengeClaimRequest request,
            [FromRoute, BindRequired] int challengeId,
            CancellationToken cancellationToken = default)
        {
            ChallengeClaim claim = new()
            {
                ChallengeId = challengeId.ToString(),
                ClaimantId = request.UserId,
            };

            return await _challengeService.ClaimChallengeAsync(claim, cancellationToken) switch
            {
                SuccessResult => new OkResult(),
                ErrorResult errorResult => errorResult.GenerateErrorResponse(),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }

        [HttpPatch]
        [Route("[controller]/claims({claimId})/witness")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchWitnessClaim(
            [FromBody, BindRequired] WitnessClaimRequest request,
            [FromRoute, BindRequired] int claimId,
            CancellationToken cancellationToken = default)
        {
            WitnessClaim witness = new()
            {
                ClaimId = claimId.ToString(),
                WitnessId = request.UserId,
            };

            return await _challengeService.WitnessClaimAsync(witness, cancellationToken) switch
            {
                SuccessResult => new OkResult(),
                ErrorResult errorResult => errorResult.GenerateErrorResponse(),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }
    }
}
