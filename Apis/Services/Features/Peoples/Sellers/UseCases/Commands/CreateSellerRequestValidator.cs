using FluentValidation;
using Services.Features.Peoples.Sellers.Models;
using Services.Features.Peoples.Sellers.Repositories;

namespace Services.Features.Peoples.Sellers.UseCases.Commands
{
    public class CreateSellerRequestValidator : AbstractValidator<CreateSellerRequest>
    {
        private readonly SellerDbContext _sellerDbContext;

        public CreateSellerRequestValidator(SellerDbContext sellerDbContext)
        {
            _sellerDbContext = sellerDbContext;

            RuleFor(p => p.Name).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.AddressLine1).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => new { p.Documents })
                .Must((x, cancellation) => DocumentAlreadyRegistered(x.Documents))
                .WithMessage("Document already registered");

            RuleFor(p => new { p.Name })
                .Must((x, cancellation) => NameAlreadyRegistered(x.Name))
                .WithMessage("Name already registered");

            RuleFor(p => new { p.Email })
                .Must((x, cancellation) => EmailAlreadyRegistered(x.Email))
                .WithMessage("Email already registered");
        }

        private bool DocumentAlreadyRegistered(
            IEnumerable<SellerRequest.SellerDocumentRequest> documents
        )
        {
            var found = false;

            documents
                .ToList()
                .ForEach(document =>
                {
                    var response = _sellerDbContext.Sellers.Where(seller =>
                        seller.Documents.Any(d =>
                            d.DocumentTypeId == document.DocumentTypeId
                            && d.EnrollmentCode == document.EnrollmentCode
                        )
                    );

                    if (response.Any())
                    {
                        found = true;
                    }
                });

            return !found;
        }

        private bool NameAlreadyRegistered(string name)
        {
            var response = _sellerDbContext.Sellers.Where(seller => seller.Name.Equals(name));
            return !response.Any();
        }

        private bool EmailAlreadyRegistered(string email)
        {
            var response = _sellerDbContext.Sellers.Where(seller => seller.Email.Equals(email));
            return !response.Any();
        }
    }
}
