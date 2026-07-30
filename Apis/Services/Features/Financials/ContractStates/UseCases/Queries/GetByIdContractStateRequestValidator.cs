using FluentValidation;

namespace Services.Features.Financials.ContractStates.UseCases.Queries
{
    public class GetByIdContractStateRequestValidator
        : AbstractValidator<GetByIdContractStateRequest>
    {
        public GetByIdContractStateRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
