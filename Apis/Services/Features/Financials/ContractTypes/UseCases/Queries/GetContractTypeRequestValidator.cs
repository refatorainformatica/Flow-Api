using FluentValidation;

namespace Services.Features.Financials.ContractTypes.UseCases.Queries
{
    public class GetContractTypeRequestValidator : AbstractValidator<GetContractTypeRequest>
    {
        public GetContractTypeRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
