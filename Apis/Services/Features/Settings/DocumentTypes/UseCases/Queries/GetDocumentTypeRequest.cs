using MediatR;
using Services.Features.Settings.DocumentTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Settings.DocumentTypes.UseCases.Queries
{
    public class GetDocumentTypeRequest
        : IRequest<Result<Response<IEnumerable<DocumentTypeResponse>>>>
    {
        public BaseQuery Query { get; set; }
    }
}
