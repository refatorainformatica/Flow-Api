namespace Services.Features.Peoples.Careers.Models.Dtos
{
    public class CareerDto
    {
        public int Id { get; set; }
        public string ExternalCode { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? EditedAt { get; set; }
        public string EditedBy { get; set; }
        public DateTime? DeletedAt { get; set; } = null;
    }
}
