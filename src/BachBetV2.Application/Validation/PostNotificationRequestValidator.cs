using BachBetV2.Domain.Models.Requests;
using FluentValidation;

namespace BachBetV2.Application.Validation
{
    public sealed class PostNotificationRequestValidator : AbstractValidator<PostNotificationRequest>
    {
        public PostNotificationRequestValidator()
        {
            RuleFor(pnr => pnr.Message).NotEmpty().WithMessage("Message cannot be empty");
        }
    }
}
