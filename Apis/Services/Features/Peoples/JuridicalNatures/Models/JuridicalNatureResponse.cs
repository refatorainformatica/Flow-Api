using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.JuridicalNatures.Models
{
    public class JuridicalNatureResponse : BaseResponse
    {
        public string ExternalCode { get; set; }
        public string Description { get; set; }
    }
}
