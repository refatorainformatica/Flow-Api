using FluentValidation;

namespace Services.Features.Financials.RevenueTypes.UseCases.Commands
{
    public class EditRevenueTypeRequestValidator : AbstractValidator<EditRevenueTypeRequest>
    {
        public EditRevenueTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
