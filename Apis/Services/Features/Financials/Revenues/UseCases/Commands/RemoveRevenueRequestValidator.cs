using FluentValidation;

namespace Services.Features.Financials.Revenues.UseCases.Commands
{
    public class RemoveRevenueRequestValidator : AbstractValidator<RemoveRevenueRequest>
    {
        public RemoveRevenueRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
