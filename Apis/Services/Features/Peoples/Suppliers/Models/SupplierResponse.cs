using Services.Features.Peoples.ActivityBranchs.Models;
using Services.Features.Peoples.JuridicalNatures.Models;
using Services.Features.Peoples.Talents.Models;
using Services.Features.Settings.DocumentTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Suppliers.Models
{
    public class SupplierResponse : BaseResponse
    {
        public string CompanyName { get; set; }
        public string CompanyBusinessName { get; set; }
        public int? JuridicalNatureId { get; set; }
        public JuridicalNatureResponse JuridicalNature { get; set; }
        public DateTime? OpeningDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public int? ActivityBranchId { get; set; }
        public ActivityBranchResponse ActivityBranch { get; set; }
        public int? PaymentingCurrencyTypeId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? TalentId { get; set; }
        public TalentResponse Talent { get; set; }

        public ICollection<SupplierDocumentResponse> Documents { get; set; } = [];

        public class SupplierDocumentResponse : BaseResponse
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
