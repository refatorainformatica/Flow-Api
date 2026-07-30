using FluentValidation;

namespace Services.Features.Peoples.Suppliers.UseCases.Queries
{
    public class GetByIdSupplierRequestValidator : AbstractValidator<GetByIdSupplierRequest>
    {
        public GetByIdSupplierRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
