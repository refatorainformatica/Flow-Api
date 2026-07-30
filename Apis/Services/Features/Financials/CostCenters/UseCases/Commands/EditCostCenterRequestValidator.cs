using FluentValidation;

namespace Services.Features.Financials.CostCenters.UseCases.Commands
{
    public class EditCostCenterRequestValidator : AbstractValidator<EditCostCenterRequest>
    {
        public EditCostCenterRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
