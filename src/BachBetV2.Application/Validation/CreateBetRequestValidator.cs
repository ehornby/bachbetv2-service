using BachBetV2.Domain.Models.Requests;
using FluentValidation;

namespace BachBetV2.Application.Validation
{
    public class CreateBetRequestValidator : AbstractValidator<CreateBetRequest>
    {
        public CreateBetRequestValidator()
        {
            RuleFor(cbr => cbr.UserId).NotEmpty();
            RuleFor(cbr => cbr.Description).NotEmpty();
            RuleFor(cbr => cbr.Odds).GreaterThan(0m).WithMessage("odds have to be more than zero fish boy");
            //RuleFor(cbr => cbr.Expiry).Must(e => DateTime.Compare(e, DateTime.Now) > 0).WithMessage("Bet expiry cannot be in the past.");
        }
    }
}
