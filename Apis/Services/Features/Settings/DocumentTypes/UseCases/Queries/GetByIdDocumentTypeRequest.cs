using MediatR;
using Services.Features.Settings.DocumentTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Settings.DocumentTypes.UseCases.Queries
{
    public class GetByIdDocumentTypeRequest : IRequest<Result<Response<DocumentTypeResponse>>>
    {
        public int Id { get; set; }
    }
}
