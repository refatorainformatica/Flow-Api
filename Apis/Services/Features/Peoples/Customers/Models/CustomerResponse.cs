using Services.Features.Settings.DocumentTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Customers.Models
{
    public class CustomerResponse : BaseResponse
    {
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<CustomerDocumentResponse> Documents { get; set; } = [];

        public class CustomerDocumentResponse : BaseResponse
        {
            public int DocumentTypeId { get; set; }
            public DocumentTypeResponse DocumentType { get; set; }
            public int CustomerId { get; set; }
            public string ExternalCode { get; set; }
            public string EnrollmentCode { get; set; }
            public DateTime EnrollmentDate { get; set; }
            public string File { get; set; }
        }
    }
}
