using FluentValidation;

namespace Services.Features.Peoples.Customers.UseCases.Queries
{
    public class GetByIdCustomerRequestValidator : AbstractValidator<GetByIdCustomerRequest>
    {
        public GetByIdCustomerRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
