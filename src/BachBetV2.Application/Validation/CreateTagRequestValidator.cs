using BachBetV2.Domain.Models.Requests;
using FluentValidation;

namespace BachBetV2.Application.Validation
{
    public class CreateTagRequestValidator : AbstractValidator<CreateTagRequest>
    {
        public CreateTagRequestValidator()
        {
            RuleFor(ctr => ctr.Description).NotEmpty().WithMessage("no blank tags bud");
        }
    }
}
