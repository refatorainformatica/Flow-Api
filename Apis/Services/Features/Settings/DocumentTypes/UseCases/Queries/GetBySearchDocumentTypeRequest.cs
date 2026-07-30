using MediatR;
using Services.Features.Settings.DocumentTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Settings.DocumentTypes.UseCases.Queries
{
    public class GetBySearchDocumentTypeRequest
        : IRequest<Result<Response<IEnumerable<DocumentTypeResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
