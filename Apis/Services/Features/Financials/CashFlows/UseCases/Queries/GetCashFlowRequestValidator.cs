using FluentValidation;

namespace Services.Features.Financials.CashFlows.UseCases.Queries
{
    public class GetCashFlowRequestValidator : AbstractValidator<GetCashFlowRequest>
    {
        public GetCashFlowRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
