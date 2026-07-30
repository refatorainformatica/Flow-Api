using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Settings.DocumentTypes.Models
{
    public class DocumentTypeResponse : BaseResponse
    {
        public string ExternalCode { get; set; }
        public string Description { get; set; }
    }
}
