using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.ActivityBranchs.Models
{
    public class ActivityBranchResponse : BaseResponse
    {
        public string ExternalCode { get; set; }
        public string Description { get; set; }
    }
}
