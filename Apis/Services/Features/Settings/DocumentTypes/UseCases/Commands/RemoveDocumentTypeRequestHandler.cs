using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Settings.DocumentTypes.Exceptions;
using Services.Features.Settings.DocumentTypes.Models;
using Services.Features.Settings.DocumentTypes.Models.Events;
using Services.Features.Settings.DocumentTypes.Repositories;
using Services.Features.Settings.DocumentTypes.UseCases.Commands;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.DocumentTypes.UseCases.Commands
{
    public class RemoveDocumentTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        DocumentTypeDbContext documenttypeDbContext
    )
        : CommandHandler(documenttypeDbContext, mediator),
            IRequestHandler<RemoveDocumentTypeRequest, Result<Response<DocumentTypeResponse>>>
    {
        private readonly DocumentTypeDbContext _documenttypeDbContext = documenttypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<DocumentTypeResponse>>> Handle(
            RemoveDocumentTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentDocumentTypeAsync(req.Id, cancellationToken))
                .BindAsync(currentDocumentType =>
                    RemoveDocumentTypeAsync(currentDocumentType, cancellationToken)
                )
                .MapAsync(currentDocumentType =>
                {
                    return new Response<DocumentTypeResponse>(null);
                });
        }

        private static Result<RemoveDocumentTypeRequest> ValidateRequest(
            RemoveDocumentTypeRequest request
        )
        {
            return request.Id == default
                ? Result<RemoveDocumentTypeRequest>.Failure(DocumentTypeErrors.NotFound(request.Id))
                : Result<RemoveDocumentTypeRequest>.Success(request);
        }

        private async Task<Result<DocumentType>> GetCurrentDocumentTypeAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var documenttype = await _documenttypeDbContext
                .DocumentTypes.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return documenttype is null
                ? Result<DocumentType>.Failure(DocumentTypeErrors.NotFound(id))
                : Result<DocumentType>.Success(documenttype);
        }

        private async Task<Result<DocumentType>> RemoveDocumentTypeAsync(
            DocumentType removeDocumentType,
            CancellationToken cancellationToken
        )
        {
            removeDocumentType.DeletedAt = _dateTimeService.UtcNow;
            removeDocumentType.EditedAt = _dateTimeService.UtcNow;
            removeDocumentType.EditedBy = _authenticatedUserService.UserId;

            removeDocumentType.AddEvent(new DocumentTypeRemovedEvent(removeDocumentType.Id));

            await ExecuteTransactionAsync(
                () => _documenttypeDbContext.Update(removeDocumentType),
                removeDocumentType.GetEvents(),
                cancellationToken
            );

            return Result<DocumentType>.Success(removeDocumentType);
        }
    }
}
