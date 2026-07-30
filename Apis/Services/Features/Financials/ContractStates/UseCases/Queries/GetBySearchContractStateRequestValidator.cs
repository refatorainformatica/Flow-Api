using FluentValidation;

namespace Services.Features.Financials.ContractStates.UseCases.Queries
{
    public class GetBySearchContractStateRequestValidator
        : AbstractValidator<GetBySearchContractStateRequest>
    {
        public GetBySearchContractStateRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
