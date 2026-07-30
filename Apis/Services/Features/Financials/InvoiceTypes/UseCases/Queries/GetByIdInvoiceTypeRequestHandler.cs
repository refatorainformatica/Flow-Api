using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.InvoiceTypes.Exceptions;
using Services.Features.Financials.InvoiceTypes.Models;
using Services.Features.Financials.InvoiceTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.InvoiceTypes.UseCases.Queries
{
    public class GetByIdInvoiceTypeRequestHandler(
        IMapper mapper,
        IMediator mediator,
        InvoiceTypeDbContext invoicetypeDbContext
    )
        : CommandHandler(invoicetypeDbContext, mediator),
            IRequestHandler<GetByIdInvoiceTypeRequest, Result<Response<InvoiceTypeResponse>>>
    {
        private readonly InvoiceTypeDbContext _invoicetypeDbContext = invoicetypeDbContext;

        public async Task<Result<Response<InvoiceTypeResponse>>> Handle(
            GetByIdInvoiceTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdInvoiceTypeAsync(request, cancellationToken)
                .BindAsync(invoicetypes => Task.FromResult(GenerateResponse(invoicetypes)));
        }

        private async Task<Result<InvoiceType>> GetByIdInvoiceTypeAsync(
            GetByIdInvoiceTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var invoicetype = await _invoicetypeDbContext
                .InvoiceTypes.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return invoicetype is null
                ? Result<InvoiceType>.Failure(InvoiceTypeErrors.NotFound(request.Id))
                : Result<InvoiceType>.Success(invoicetype);
        }

        private Result<Response<InvoiceTypeResponse>> GenerateResponse(InvoiceType invoicetype)
        {
            var invoicetypeResponse = mapper.Map<InvoiceTypeResponse>(invoicetype);
            var response = new Response<InvoiceTypeResponse>(invoicetypeResponse);
            return Result<Response<InvoiceTypeResponse>>.Success(response);
        }
    }
}
