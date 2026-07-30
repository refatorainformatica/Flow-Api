using FluentValidation;

namespace Services.Features.Financials.Banks.UseCases.Commands
{
    public class RemoveBankRequestValidator : AbstractValidator<RemoveBankRequest>
    {
        public RemoveBankRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
