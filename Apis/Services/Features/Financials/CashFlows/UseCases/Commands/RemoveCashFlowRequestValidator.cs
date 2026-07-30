using FluentValidation;

namespace Services.Features.Financials.CashFlows.UseCases.Commands
{
    public class RemoveCashFlowRequestValidator : AbstractValidator<RemoveCashFlowRequest>
    {
        public RemoveCashFlowRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
