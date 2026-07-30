namespace Services.Features.Peoples.Customers.Models
{
    public class CustomerRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Picture { get; set; }

        public ICollection<CustomerDocumentRequest> Documents { get; set; } = [];

        public class CustomerDocumentRequest
        {
            public int? Id { get; set; }
            public int DocumentTypeId { get; set; }
            public int CustomerId { get; set; }
            public string ExternalCode { get; set; }
            public string EnrollmentCode { get; set; }
            public DateTime EnrollmentDate { get; set; }
            public string File { get; set; }
            public string Picture { get; set; }
            public DateTime? DeletedAt { get; set; }
        }
    }
}
