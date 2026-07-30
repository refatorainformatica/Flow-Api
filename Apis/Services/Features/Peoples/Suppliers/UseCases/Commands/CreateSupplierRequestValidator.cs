using FluentValidation;
using Services.Features.Peoples.Suppliers.Models;
using Services.Features.Peoples.Suppliers.Repositories;

namespace Services.Features.Peoples.Suppliers.UseCases.Commands
{
    public class CreateSupplierRequestValidator : AbstractValidator<CreateSupplierRequest>
    {
        private readonly SupplierDbContext _supplierDbContext;

        public CreateSupplierRequestValidator(SupplierDbContext supplierDbContext)
        {
            _supplierDbContext = supplierDbContext;

            RuleFor(p => p.CompanyName).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.CompanyBusinessName)
                .NotEmpty()
                .WithMessage("{PropertyName} is required");

            RuleFor(p => p.AddressLine1).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => new { p.Documents })
                .Must((x, cancellation) => DocumentAlreadyRegistered(x.Documents))
                .WithMessage("Document already registered");

            RuleFor(p => new { p.CompanyName })
                .Must((x, cancellation) => CompanyNameAlreadyRegistered(x.CompanyName))
                .WithMessage("Company name already registered");

            RuleFor(p => new { p.CompanyBusinessName })
                .Must(
                    (x, cancellation) => CompanyBusinessNameAlreadyRegistered(x.CompanyBusinessName)
                )
                .WithMessage("Company business name already registered");

            RuleFor(p => new { p.Email })
                .Must((x, cancellation) => EmailAlreadyRegistered(x.Email))
                .WithMessage("Email already registered");
        }

        private bool DocumentAlreadyRegistered(
            IEnumerable<SupplierRequest.SupplierDocumentRequest> documents
        )
        {
            var found = false;

            documents
                .ToList()
                .ForEach(document =>
                {
                    var response = _supplierDbContext.Suppliers.Where(supplier =>
                        supplier.Documents.Any(d =>
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

        private bool CompanyNameAlreadyRegistered(string companyName)
        {
            var response = _supplierDbContext.Suppliers.Where(supplier =>
                supplier.CompanyName.Equals(companyName)
            );
            return !response.Any();
        }

        private bool CompanyBusinessNameAlreadyRegistered(string companyBusinessName)
        {
            var response = _supplierDbContext.Suppliers.Where(supplier =>
                supplier.CompanyBusinessName.Equals(companyBusinessName)
            );
            return !response.Any();
        }

        private bool EmailAlreadyRegistered(string email)
        {
            var response = _supplierDbContext.Suppliers.Where(supplier =>
                supplier.Email.Equals(email)
            );
            return !response.Any();
        }
    }
}
