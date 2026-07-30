using FluentValidation;

namespace Services.Features.Financials.ContractTypes.UseCases.Queries
{
    public class GetBySearchContractTypeRequestValidator
        : AbstractValidator<GetBySearchContractTypeRequest>
    {
        public GetBySearchContractTypeRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
