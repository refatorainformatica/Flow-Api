namespace Services.Features.Peoples.Talents.Models
{
    public class TalentRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string Nationality { get; set; }
        public string Naturalness { get; set; }
        public bool LivesAbroad { get; set; }
        public bool RemoteJob { get; set; }
        public short NumberOfChildren { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public int? CareerId { get; set; }
        public int? ProfessionalCategoryd { get; set; }
        public decimal ValueOfServices { get; set; }
        public string ConsortName { get; set; }
        public int? MaritalStateId { get; set; }
        public int? EducationLevelId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Email { get; set; }
        public string CorporateEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string EmergencyContact { get; set; }
        public string LinkedIn { get; set; }
        public string Project { get; set; }
        public bool Fired { get; set; }
        public string ResignationOpinion { get; set; }
        public string Picture { get; set; }

        public ICollection<TalentDocumentRequest> Documents { get; set; } = [];
        public ICollection<TalentSkillRequest> Skills { get; set; } = [];

        public class TalentDocumentRequest
        {
            public int? Id { get; set; }
            public int DocumentTypeId { get; set; }
            public int TalentId { get; set; }
            public string ExternalCode { get; set; }
            public string EnrollmentCode { get; set; }
            public DateTime EnrollmentDate { get; set; }
            public string File { get; set; }
            public string Picture { get; set; }
            public DateTime? DeletedAt { get; set; }
        }

        public class TalentSkillRequest
        {
            public int? Id { get; set; }
            public int TalentId { get; set; }
            public string Description { get; set; }
            public string Institute { get; set; }
            public int SkillTypeId { get; set; }
            public int SkillCategoryId { get; set; }
            public int SkillLevelId { get; set; }
            public int SkillLevelMaxId { get; set; }
            public int SkillStateId { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Picture { get; set; }
            public DateTime? DeletedAt { get; set; }
        }
    }
}
