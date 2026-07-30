using FluentValidation;

namespace Services.Features.Financials.CashFlows.UseCases.Queries
{
    public class GetByIdCashFlowRequestValidator : AbstractValidator<GetByIdCashFlowRequest>
    {
        public GetByIdCashFlowRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
