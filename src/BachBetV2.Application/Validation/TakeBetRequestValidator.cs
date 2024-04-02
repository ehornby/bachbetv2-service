using BachBetV2.Domain.Models.Requests;
using FluentValidation;

namespace BachBetV2.Application.Validation
{
    public class TakeBetRequestValidator : AbstractValidator<TakeBetRequest>
    {
        public TakeBetRequestValidator()
        {
            RuleFor(tbr => tbr.Wager).GreaterThan(0m).WithMessage("can't bet zero idiot");
        }
    }
}
