using FluentValidation;

namespace Services.Features.Financials.Contracts.UseCases.Queries
{
    public class GetBySearchContractRequestValidator : AbstractValidator<GetBySearchContractRequest>
    {
        public GetBySearchContractRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
