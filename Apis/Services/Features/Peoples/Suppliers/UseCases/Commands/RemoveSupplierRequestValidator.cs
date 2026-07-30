using FluentValidation;

namespace Services.Features.Peoples.Suppliers.UseCases.Commands
{
    public class RemoveSupplierRequestValidator : AbstractValidator<RemoveSupplierRequest>
    {
        public RemoveSupplierRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
