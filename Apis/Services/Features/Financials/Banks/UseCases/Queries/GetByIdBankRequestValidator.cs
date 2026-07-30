using FluentValidation;

namespace Services.Features.Financials.Banks.UseCases.Queries
{
    public class GetByIdBankRequestValidator : AbstractValidator<GetByIdBankRequest>
    {
        public GetByIdBankRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
