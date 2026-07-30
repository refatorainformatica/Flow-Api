using FluentValidation;

namespace Services.Features.Financials.Contracts.UseCases.Queries
{
    public class GetByIdContractRequestValidator : AbstractValidator<GetByIdContractRequest>
    {
        public GetByIdContractRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
