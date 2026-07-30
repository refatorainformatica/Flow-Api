using FluentValidation;

namespace Services.Features.Financials.RevenueTypes.UseCases.Commands
{
    public class CreateRevenueTypeRequestValidator : AbstractValidator<CreateRevenueTypeRequest>
    {
        public CreateRevenueTypeRequestValidator()
        {
            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
