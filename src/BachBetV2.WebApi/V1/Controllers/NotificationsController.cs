using BachBetV2.Application.Enums;
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
    [Route("v1/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IPushNotificationService _notificationService;

        public NotificationsController(IPushNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostNotification(
            [FromBody, BindRequired] PostNotificationRequest request,
            [FromServices] IValidator<PostNotificationRequest> validator,
            CancellationToken cancellationToken = default)
        {
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors);
            }

            PushNotification notification = new()
            {
                Message = request.Message,
                RelatedUserId = request.UserId,
                NotificationType = NotificationType.Alert
            };

            return await _notificationService.SendManualPushNotificationAsync(notification, cancellationToken) switch
            {
                SuccessResult => new OkResult(),
                ErrorResult errorResult => errorResult.GenerateErrorResponse(),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }
    }
}
