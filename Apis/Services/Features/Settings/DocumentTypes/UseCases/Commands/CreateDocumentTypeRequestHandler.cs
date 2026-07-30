using AutoMapper;
using MediatR;
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
    public class CreateDocumentTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        DocumentTypeDbContext documenttypeDbContext
    )
        : CommandHandler(documenttypeDbContext, mediator),
            IRequestHandler<CreateDocumentTypeRequest, Result<Response<DocumentTypeResponse>>>
    {
        private readonly DocumentTypeDbContext _documenttypeDbContext = documenttypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<DocumentTypeResponse>>> Handle(
            CreateDocumentTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveDocumentTypeAsync(request, cancellationToken)
                .BindAsync(documenttype => Task.FromResult(GenerateResponse(documenttype)));
        }

        private async Task<Result<DocumentType>> SaveDocumentTypeAsync(
            CreateDocumentTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var newDocumentType = new DocumentType(
                0,
                request.ExternalCode,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.DocumentBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newDocumentType.AddEvent(new DocumentTypeCreatedEvent(newDocumentType.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _documenttypeDbContext.DocumentTypes.AddAsync(
                        newDocumentType,
                        cancellationToken: cancellationToken
                    );
                },
                newDocumentType.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<DocumentType>.Success(newDocumentType);
        }

        private Result<Response<DocumentTypeResponse>> GenerateResponse(DocumentType documenttype)
        {
            var documenttypeResponse = mapper.Map<DocumentTypeResponse>(documenttype);
            var response = new Response<DocumentTypeResponse>(documenttypeResponse);

            return Result<Response<DocumentTypeResponse>>.Success(response);
        }
    }
}
