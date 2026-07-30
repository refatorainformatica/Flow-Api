using FluentValidation;
using Services.Features.Peoples.Customers.Models;
using Services.Features.Peoples.Customers.Repositories;

namespace Services.Features.Peoples.Customers.UseCases.Commands
{
    public class EditCustomerRequestValidator : AbstractValidator<EditCustomerRequest>
    {
        private readonly CustomerDbContext _customerDbContext;

        public EditCustomerRequestValidator(CustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext;

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
            IEnumerable<CustomerRequest.CustomerDocumentRequest> documents
        )
        {
            var found = false;

            documents
                .ToList()
                .ForEach(document =>
                {
                    var response = _customerDbContext.Customers.Where(customer =>
                        customer.Documents.Any(d =>
                            d.DocumentTypeId == document.DocumentTypeId
                            && d.EnrollmentCode == document.EnrollmentCode
                        )
                        && customer.Id != id
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
            var response = _customerDbContext.Customers.Where(customer =>
                customer.Id != id && customer.Name.Equals(name)
            );
            return !response.Any();
        }

        private bool EmailAlreadyRegistered(int id, string email)
        {
            var response = _customerDbContext.Customers.Where(customer =>
                customer.Id != id && customer.Email.Equals(email)
            );
            return !response.Any();
        }
    }
}
