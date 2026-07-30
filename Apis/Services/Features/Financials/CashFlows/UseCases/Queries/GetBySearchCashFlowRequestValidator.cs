using FluentValidation;

namespace Services.Features.Financials.CashFlows.UseCases.Queries
{
    public class GetBySearchCashFlowRequestValidator : AbstractValidator<GetBySearchCashFlowRequest>
    {
        public GetBySearchCashFlowRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
