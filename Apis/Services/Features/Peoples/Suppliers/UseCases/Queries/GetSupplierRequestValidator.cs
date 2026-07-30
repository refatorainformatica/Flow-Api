using FluentValidation;

namespace Services.Features.Peoples.Suppliers.UseCases.Queries
{
    public class GetSupplierRequestValidator : AbstractValidator<GetSupplierRequest>
    {
        public GetSupplierRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
