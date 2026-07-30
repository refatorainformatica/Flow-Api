namespace Services.Features.Peoples.EducationLevels.Models
{
    public class EducationLevelRequest
    {
        public int Id { get; set; }
        public string ExternalCode { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
    }
}
