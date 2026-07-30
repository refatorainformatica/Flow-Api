using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.MaritalStates.Models
{
    public class MaritalStateResponse : BaseResponse
    {
        public string Description { get; set; }
    }
}
