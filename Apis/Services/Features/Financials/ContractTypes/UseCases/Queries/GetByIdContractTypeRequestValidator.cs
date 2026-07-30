using FluentValidation;

namespace Services.Features.Financials.ContractTypes.UseCases.Queries
{
    public class GetByIdContractTypeRequestValidator : AbstractValidator<GetByIdContractTypeRequest>
    {
        public GetByIdContractTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
