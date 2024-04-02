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
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionsController(ISubscriptionService subscriptionService)

        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostSubscription([FromBody, BindRequired] CreateSubscriptionRequest request, CancellationToken cancellationToken = default)
        {
            Subscription pushSubscription = new()
            {
                UserId = request.UserId,
                Auth = request.Auth,
                P256DH = request.P256DH,
                PushEndpoint = request.PushEndpoint,
                PublicKey = request.PublicKey,
                PrivateKey = request.PrivateKey
            };

            return await _subscriptionService.AddSubscriptionAsync(pushSubscription, cancellationToken) switch
            {
                SuccessResult => new OkResult(),
                ErrorResult errorResult => errorResult.GenerateErrorResponse(),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }
    }
}
