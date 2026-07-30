using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.ProfessionalProfiles.Models
{
    public class ProfessionalProfileResponse : BaseResponse
    {
        public string Description { get; set; }
        public decimal HourlyValue { get; set; }
        public decimal OvertimeValue { get; set; }
        public decimal AdditionalHourlyValue { get; set; }
    }
}
