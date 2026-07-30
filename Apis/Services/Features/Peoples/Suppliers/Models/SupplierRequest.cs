namespace Services.Features.Peoples.Suppliers.Models
{
    public class SupplierRequest
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyBusinessName { get; set; }
        public int? JuridicalNatureId { get; set; }
        public DateTime? OpeningDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public int? ActivityBranchId { get; set; }
        public int? PaymentingCurrencyTypeId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Picture { get; set; }
        public int? TalentId { get; set; }

        public ICollection<SupplierDocumentRequest> Documents { get; set; } = [];

        public class SupplierDocumentRequest
        {
            public int? Id { get; set; }
            public int DocumentTypeId { get; set; }
            public int SupplierId { get; set; }
            public string ExternalCode { get; set; }
            public string EnrollmentCode { get; set; }
            public DateTime EnrollmentDate { get; set; }
            public string File { get; set; }
            public string Picture { get; set; }
            public DateTime? DeletedAt { get; set; }
        }
    }
}
