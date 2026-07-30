using MediatR;
using Services.Features.Settings.DocumentTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Settings.DocumentTypes.UseCases.Commands
{
    public class CreateDocumentTypeRequest
        : DocumentTypeRequest,
            IRequest<Result<Response<DocumentTypeResponse>>> { }
}
