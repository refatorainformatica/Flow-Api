using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Careers.Models
{
    public class CareerResponse : BaseResponse
    {
        public string ExternalCode { get; set; }
        public string Description { get; set; }
    }
}
