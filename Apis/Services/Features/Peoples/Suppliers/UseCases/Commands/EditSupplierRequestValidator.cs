using FluentValidation;
using Services.Features.Peoples.Suppliers.Models;
using Services.Features.Peoples.Suppliers.Repositories;

namespace Services.Features.Peoples.Suppliers.UseCases.Commands
{
    public class EditSupplierRequestValidator : AbstractValidator<EditSupplierRequest>
    {
        private readonly SupplierDbContext _supplierDbContext;

        public EditSupplierRequestValidator(SupplierDbContext supplierDbContext)
        {
            _supplierDbContext = supplierDbContext;

            RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.RequestId).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.CompanyName).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.CompanyBusinessName)
                .NotEmpty()
                .WithMessage("{PropertyName} is required");

            RuleFor(p => p.AddressLine1).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => p.Picture).NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(p => new { p.Id, p.Documents })
                .Must((x, cancellation) => DocumentAlreadyRegistered(x.Id, x.Documents))
                .WithMessage("Document already registered");

            RuleFor(p => new { p.Id, p.CompanyName })
                .Must((x, cancellation) => CompanyNameAlreadyRegistered(x.Id, x.CompanyName))
                .WithMessage("Company name already registered");

            RuleFor(p => new { p.Id, p.CompanyBusinessName })
                .Must(
                    (x, cancellation) =>
                        CompanyBusinessNameAlreadyRegistered(x.Id, x.CompanyBusinessName)
                )
                .WithMessage("Company business name already registered");

            RuleFor(p => new { p.Id, p.Email })
                .Must((x, cancellation) => EmailAlreadyRegistered(x.Id, x.Email))
                .WithMessage("Email already registered");
        }

        private bool DocumentAlreadyRegistered(
            int id,
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
                        && supplier.Id != id
                    );

                    if (response.Any())
                    {
                        found = true;
                    }
                });

            return !found;
        }

        private bool CompanyNameAlreadyRegistered(int id, string companyName)
        {
            var response = _supplierDbContext.Suppliers.Where(supplier =>
                supplier.Id != id && supplier.CompanyName.Equals(companyName)
            );
            return !response.Any();
        }

        private bool CompanyBusinessNameAlreadyRegistered(int id, string companyBusinessName)
        {
            var response = _supplierDbContext.Suppliers.Where(supplier =>
                supplier.Id != id && supplier.CompanyBusinessName.Equals(companyBusinessName)
            );
            return !response.Any();
        }

        private bool EmailAlreadyRegistered(int id, string email)
        {
            var response = _supplierDbContext.Suppliers.Where(supplier =>
                supplier.Id != id && supplier.Email.Equals(email)
            );
            return !response.Any();
        }
    }
}
