using FluentValidation;

namespace Services.Features.Financials.RevenueTypes.UseCases.Commands
{
    public class RemoveRevenueTypeRequestValidator : AbstractValidator<RemoveRevenueTypeRequest>
    {
        public RemoveRevenueTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
