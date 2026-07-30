using FluentValidation;

namespace Services.Features.Financials.Banks.UseCases.Queries
{
    public class GetBankRequestValidator : AbstractValidator<GetBankRequest>
    {
        public GetBankRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
