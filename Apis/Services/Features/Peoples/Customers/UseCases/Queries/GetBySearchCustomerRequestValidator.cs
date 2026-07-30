using FluentValidation;

namespace Services.Features.Peoples.Customers.UseCases.Queries
{
    public class GetBySearchCustomerRequestValidator : AbstractValidator<GetBySearchCustomerRequest>
    {
        public GetBySearchCustomerRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
