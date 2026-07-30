using MediatR;
using Services.Features.Settings.DocumentTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Settings.DocumentTypes.UseCases.Commands
{
    public class EditDocumentTypeRequest
        : DocumentTypeRequest,
            IRequest<Result<Response<DocumentTypeResponse>>>
    {
        public int RequestId { get; set; }
    }
}
