using FluentValidation;

namespace Services.Features.Peoples.Sellers.UseCases.Queries
{
    public class GetBySearchSellerRequestValidator : AbstractValidator<GetBySearchSellerRequest>
    {
        public GetBySearchSellerRequestValidator()
        {
            RuleFor(x => x.Query).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.Query.SearchText).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
