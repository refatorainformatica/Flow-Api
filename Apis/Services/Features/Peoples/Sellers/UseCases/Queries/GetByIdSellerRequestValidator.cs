using FluentValidation;

namespace Services.Features.Peoples.Sellers.UseCases.Queries
{
    public class GetByIdSellerRequestValidator : AbstractValidator<GetByIdSellerRequest>
    {
        public GetByIdSellerRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
