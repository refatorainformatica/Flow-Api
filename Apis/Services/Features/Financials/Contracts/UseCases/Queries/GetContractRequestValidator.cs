using FluentValidation;

namespace Services.Features.Financials.Contracts.UseCases.Queries
{
    public class GetContractRequestValidator : AbstractValidator<GetContractRequest>
    {
        public GetContractRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
