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
    public class EditDocumentTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        DocumentTypeDbContext documenttypeDbContext
    )
        : CommandHandler(documenttypeDbContext, mediator),
            IRequestHandler<EditDocumentTypeRequest, Result<Response<DocumentTypeResponse>>>
    {
        private readonly DocumentTypeDbContext _documenttypeDbContext = documenttypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<DocumentTypeResponse>>> Handle(
            EditDocumentTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentDocumentTypeAsync(req.Id, cancellationToken))
                .BindAsync(currentDocumentType =>
                    EditAndSaveDocumentTypeAsync(currentDocumentType, request, cancellationToken)
                )
                .MapAsync(currentDocumentType =>
                {
                    return new Response<DocumentTypeResponse>(null);
                });
        }

        private static Result<EditDocumentTypeRequest> ValidateRequest(
            EditDocumentTypeRequest request
        )
        {
            return request.Id != request.RequestId
                ? Result<EditDocumentTypeRequest>.Failure(
                    DocumentTypeErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditDocumentTypeRequest>.Success(request);
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

        private async Task<Result<DocumentType>> EditAndSaveDocumentTypeAsync(
            DocumentType currentDocumentType,
            EditDocumentTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var editDocumentType = new DocumentType(
                request.Id,
                request.ExternalCode,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.DocumentBase64Image,
                currentDocumentType.CreatedAt.GetValueOrDefault(),
                currentDocumentType.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editDocumentType.AddEvent(new DocumentTypeEditedEvent(editDocumentType.Id));

            await ExecuteTransactionAsync(
                () => _documenttypeDbContext.DocumentTypes.Update(editDocumentType),
                editDocumentType.GetEvents(),
                cancellationToken
            );

            return Result<DocumentType>.Success(editDocumentType);
        }
    }
}
