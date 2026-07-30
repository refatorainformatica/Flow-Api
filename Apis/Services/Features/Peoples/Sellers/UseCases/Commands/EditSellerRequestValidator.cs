using FluentValidation;
using Services.Features.Peoples.Sellers.Models;
using Services.Features.Peoples.Sellers.Repositories;

namespace Services.Features.Peoples.Sellers.UseCases.Commands
{
    public class EditSellerRequestValidator : AbstractValidator<EditSellerRequest>
    {
        private readonly SellerDbContext _sellerDbContext;

        public EditSellerRequestValidator(SellerDbContext sellerDbContext)
        {
            _sellerDbContext = sellerDbContext;

            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Name).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.AddressLine1).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => new { p.Id, p.Documents })
                .Must((x, cancellation) => DocumentAlreadyRegistered(x.Id, x.Documents))
                .WithMessage("Document already registered");

            RuleFor(p => new { p.Id, p.Name })
                .Must((x, cancellation) => NameAlreadyRegistered(x.Id, x.Name))
                .WithMessage("Name already registered");

            RuleFor(p => new { p.Id, p.Email })
                .Must((x, cancellation) => EmailAlreadyRegistered(x.Id, x.Email))
                .WithMessage("Email already registered");
        }

        private bool DocumentAlreadyRegistered(
            int id,
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
                        && seller.Id != id
                    );

                    if (response.Any())
                    {
                        found = true;
                    }
                });

            return !found;
        }

        private bool NameAlreadyRegistered(int id, string name)
        {
            var response = _sellerDbContext.Sellers.Where(seller =>
                seller.Id != id && seller.Name.Equals(name)
            );
            return !response.Any();
        }

        private bool EmailAlreadyRegistered(int id, string email)
        {
            var response = _sellerDbContext.Sellers.Where(seller =>
                seller.Id != id && seller.Email.Equals(email)
            );
            return !response.Any();
        }
    }
}
