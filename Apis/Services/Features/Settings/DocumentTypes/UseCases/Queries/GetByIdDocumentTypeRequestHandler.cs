using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Settings.DocumentTypes.Exceptions;
using Services.Features.Settings.DocumentTypes.Models;
using Services.Features.Settings.DocumentTypes.Repositories;
using Services.Features.Settings.DocumentTypes.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.DocumentTypes.UseCases.Queries
{
    public class GetByIdDocumentTypeRequestHandler(
        IMapper mapper,
        IMediator mediator,
        DocumentTypeDbContext documenttypeDbContext
    )
        : CommandHandler(documenttypeDbContext, mediator),
            IRequestHandler<GetByIdDocumentTypeRequest, Result<Response<DocumentTypeResponse>>>
    {
        private readonly DocumentTypeDbContext _documenttypeDbContext = documenttypeDbContext;

        public async Task<Result<Response<DocumentTypeResponse>>> Handle(
            GetByIdDocumentTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdDocumentTypeAsync(request, cancellationToken)
                .BindAsync(documenttypes => Task.FromResult(GenerateResponse(documenttypes)));
        }

        private async Task<Result<DocumentType>> GetByIdDocumentTypeAsync(
            GetByIdDocumentTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var documenttype = await _documenttypeDbContext
                .DocumentTypes.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return documenttype is null
                ? Result<DocumentType>.Failure(DocumentTypeErrors.NotFound(request.Id))
                : Result<DocumentType>.Success(documenttype);
        }

        private Result<Response<DocumentTypeResponse>> GenerateResponse(DocumentType documenttype)
        {
            var documenttypeResponse = mapper.Map<DocumentTypeResponse>(documenttype);
            var response = new Response<DocumentTypeResponse>(documenttypeResponse);
            return Result<Response<DocumentTypeResponse>>.Success(response);
        }
    }
}
