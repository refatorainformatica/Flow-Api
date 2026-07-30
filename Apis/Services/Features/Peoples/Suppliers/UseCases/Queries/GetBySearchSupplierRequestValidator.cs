using FluentValidation;

namespace Services.Features.Peoples.Suppliers.UseCases.Queries
{
    public class GetBySearchSupplierRequestValidator : AbstractValidator<GetBySearchSupplierRequest>
    {
        public GetBySearchSupplierRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
