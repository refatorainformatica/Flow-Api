using FluentValidation;

namespace Services.Features.Peoples.Customers.UseCases.Queries
{
    public class GetCustomerRequestValidator : AbstractValidator<GetCustomerRequest>
    {
        public GetCustomerRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
