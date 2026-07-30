using MediatR;
using Services.Features.Settings.DocumentTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Settings.DocumentTypes.UseCases.Commands
{
    public class RemoveDocumentTypeRequest : IRequest<Result<Response<DocumentTypeResponse>>>
    {
        public int Id { get; set; }
    }
}
