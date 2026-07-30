using Services.Features.Settings.DocumentTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Sellers.Models
{
    public class SellerResponse : BaseResponse
    {
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<SellerDocumentResponse> Documents { get; set; } = [];

        public class SellerDocumentResponse : BaseResponse
        {
            public int DocumentTypeId { get; set; }
            public DocumentTypeResponse DocumentType { get; set; }
            public int SellerId { get; set; }
            public string ExternalCode { get; set; }
            public string EnrollmentCode { get; set; }
            public DateTime EnrollmentDate { get; set; }
            public string File { get; set; }
        }
    }
}
