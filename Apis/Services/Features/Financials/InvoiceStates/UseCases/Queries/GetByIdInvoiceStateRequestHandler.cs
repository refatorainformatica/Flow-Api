using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.InvoiceStates.Exceptions;
using Services.Features.Financials.InvoiceStates.Models;
using Services.Features.Financials.InvoiceStates.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.InvoiceStates.UseCases.Queries
{
    public class GetByIdInvoiceStateRequestHandler(
        IMapper mapper,
        IMediator mediator,
        InvoiceStateDbContext invoicestateDbContext
    )
        : CommandHandler(invoicestateDbContext, mediator),
            IRequestHandler<GetByIdInvoiceStateRequest, Result<Response<InvoiceStateResponse>>>
    {
        private readonly InvoiceStateDbContext _invoicestateDbContext = invoicestateDbContext;

        public async Task<Result<Response<InvoiceStateResponse>>> Handle(
            GetByIdInvoiceStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdInvoiceStateAsync(request, cancellationToken)
                .BindAsync(invoicestates => Task.FromResult(GenerateResponse(invoicestates)));
        }

        private async Task<Result<InvoiceState>> GetByIdInvoiceStateAsync(
            GetByIdInvoiceStateRequest request,
            CancellationToken cancellationToken
        )
        {
            var invoicestate = await _invoicestateDbContext
                .InvoiceStates.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return invoicestate is null
                ? Result<InvoiceState>.Failure(InvoiceStateErrors.NotFound(request.Id))
                : Result<InvoiceState>.Success(invoicestate);
        }

        private Result<Response<InvoiceStateResponse>> GenerateResponse(InvoiceState invoicestate)
        {
            var invoicestateResponse = mapper.Map<InvoiceStateResponse>(invoicestate);
            var response = new Response<InvoiceStateResponse>(invoicestateResponse);
            return Result<Response<InvoiceStateResponse>>.Success(response);
        }
    }
}
