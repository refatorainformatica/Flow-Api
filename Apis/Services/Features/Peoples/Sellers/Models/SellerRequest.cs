namespace Services.Features.Peoples.Sellers.Models
{
    public class SellerRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Picture { get; set; }

        public ICollection<SellerDocumentRequest> Documents { get; set; } = [];

        public class SellerDocumentRequest
        {
            public int? Id { get; set; }
            public int DocumentTypeId { get; set; }
            public int SellerId { get; set; }
            public string ExternalCode { get; set; }
            public string EnrollmentCode { get; set; }
            public DateTime EnrollmentDate { get; set; }
            public string File { get; set; }
            public string Picture { get; set; }
            public DateTime? DeletedAt { get; set; }
        }
    }
}
