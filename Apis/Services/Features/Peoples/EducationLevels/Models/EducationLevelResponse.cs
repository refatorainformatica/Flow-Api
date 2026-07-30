using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.EducationLevels.Models
{
    public class EducationLevelResponse : BaseResponse
    {
        public string ExternalCode { get; set; }
        public string Description { get; set; }
    }
}
