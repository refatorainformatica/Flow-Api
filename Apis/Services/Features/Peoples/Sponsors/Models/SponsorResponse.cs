using Services.Features.Settings.DocumentTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Sponsors.Models
{
    public class SponsorResponse : BaseResponse
    {
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<SponsorDocumentResponse> Documents { get; set; } = [];

        public class SponsorDocumentResponse : BaseResponse
        {
            public int DocumentTypeId { get; set; }
            public DocumentTypeResponse DocumentType { get; set; }
            public int SponsorId { get; set; }
            public string ExternalCode { get; set; }
            public string EnrollmentCode { get; set; }
            public DateTime EnrollmentDate { get; set; }
            public string File { get; set; }
        }
    }
}
