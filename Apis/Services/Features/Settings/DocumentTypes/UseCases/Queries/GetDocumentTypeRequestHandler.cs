using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Settings.DocumentTypes.Exceptions;
using Services.Features.Settings.DocumentTypes.Models;
using Services.Features.Settings.DocumentTypes.Repositories;
using Services.Features.Settings.DocumentTypes.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.DocumentTypes.UseCases.Queries
{
    public class GetDocumentTypeRequestHandler(
        IMapper mapper,
        IMediator mediator,
        DocumentTypeDbContext documenttypeDbContext
    )
        : CommandHandler(documenttypeDbContext, mediator),
            IRequestHandler<
                GetDocumentTypeRequest,
                Result<Response<IEnumerable<DocumentTypeResponse>>>
            >
    {
        private readonly DocumentTypeDbContext _documenttypeDbContext = documenttypeDbContext;

        public async Task<Result<Response<IEnumerable<DocumentTypeResponse>>>> Handle(
            GetDocumentTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetDocumentTypeAsync(request)
                .BindAsync(documenttypes => Task.FromResult(GenerateResponse(documenttypes)));
        }

        private async Task<Result<Pagination<DocumentType>>> GetDocumentTypeAsync(
            GetDocumentTypeRequest request
        )
        {
            var documenttypes = await Task.Run(
                () =>
                    _documenttypeDbContext
                        .DocumentTypes.AsNoTracking()
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<DocumentType>()
            );

            return !documenttypes.Rows.Any()
                ? Result<Pagination<DocumentType>>.Failure(DocumentTypeErrors.IsEmpty())
                : Result<Pagination<DocumentType>>.Success(documenttypes);
        }

        private Result<Response<IEnumerable<DocumentTypeResponse>>> GenerateResponse(
            Pagination<DocumentType> paginationDocumentType
        )
        {
            var documenttypeResponse = mapper.Map<IEnumerable<DocumentTypeResponse>>(
                paginationDocumentType.Rows
            );
            var response = new Response<IEnumerable<DocumentTypeResponse>>(
                documenttypeResponse,
                paginationDocumentType.Offset,
                paginationDocumentType.Limit,
                paginationDocumentType.PageCount,
                paginationDocumentType.RowCount
            );
            return Result<Response<IEnumerable<DocumentTypeResponse>>>.Success(response);
        }
    }
}
