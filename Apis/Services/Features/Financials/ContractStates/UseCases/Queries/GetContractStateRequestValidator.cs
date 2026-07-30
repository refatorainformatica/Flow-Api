using FluentValidation;

namespace Services.Features.Financials.ContractStates.UseCases.Queries
{
    public class GetContractStateRequestValidator : AbstractValidator<GetContractStateRequest>
    {
        public GetContractStateRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
