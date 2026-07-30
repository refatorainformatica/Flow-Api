namespace Services.Features.Peoples.ProfessionalProfiles.Models
{
    public class ProfessionalProfileRequest
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal HourlyValue { get; set; }
        public decimal OvertimeValue { get; set; }
        public decimal AdditionalHourlyValue { get; set; }
        public string Picture { get; set; }
    }
}
