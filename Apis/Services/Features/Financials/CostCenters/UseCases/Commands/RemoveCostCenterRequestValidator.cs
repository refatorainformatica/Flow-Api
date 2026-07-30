using FluentValidation;

namespace Services.Features.Financials.CostCenters.UseCases.Commands
{
    public class RemoveCostCenterRequestValidator : AbstractValidator<RemoveCostCenterRequest>
    {
        public RemoveCostCenterRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
