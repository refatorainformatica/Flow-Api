using MediatR;
using Services.Features.Financials.Invoices.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Invoices.UseCases.Queries
{
    public class GetBySearchInvoiceRequest
        : IRequest<Result<Response<IEnumerable<InvoiceResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
