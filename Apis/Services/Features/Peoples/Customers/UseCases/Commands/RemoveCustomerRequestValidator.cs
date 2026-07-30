using FluentValidation;

namespace Services.Features.Peoples.Customers.UseCases.Commands
{
    public class RemoveCustomerRequestValidator : AbstractValidator<RemoveCustomerRequest>
    {
        public RemoveCustomerRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
