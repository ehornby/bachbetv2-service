using BachBetV2.Domain.Models.Requests;
using FluentValidation;

namespace BachBetV2.Application.Validation
{
    public class CreateChallengeRequestValidator : AbstractValidator<CreateChallengeRequest>
    {
        public CreateChallengeRequestValidator()
        {
            RuleFor(ccr => ccr.Description).NotEmpty();
            RuleFor(ccr => ccr.Reward).GreaterThan(0m);
        }
    }
}
