using BachBetV2.Domain.Models.Requests;
using FluentValidation;

namespace BachBetV2.Application.Validation
{
    public class BalanceTransferRequestValidator : AbstractValidator<BalanceTransferRequest>
    {
        public BalanceTransferRequestValidator()
        {
            RuleFor(btr => btr.Amount).GreaterThan(0m);
            RuleFor(btr => btr.ReceivingUserId).NotEmpty();
        }
    }
}
