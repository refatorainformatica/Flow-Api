namespace Services.Features.Peoples.Sponsors.Models
{
    public class SponsorRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Picture { get; set; }

        public ICollection<SponsorDocumentRequest> Documents { get; set; } = [];

        public class SponsorDocumentRequest
        {
            public int? Id { get; set; }
            public int DocumentTypeId { get; set; }
            public int SponsorId { get; set; }
            public string ExternalCode { get; set; }
            public string EnrollmentCode { get; set; }
            public DateTime EnrollmentDate { get; set; }
            public string File { get; set; }
            public string Picture { get; set; }
            public DateTime? DeletedAt { get; set; }
        }
    }
}
