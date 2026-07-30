using MediatR;
using Services.Features.Financials.Invoices.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Invoices.UseCases.Queries
{
    public class GetInvoiceRequest : IRequest<Result<Response<IEnumerable<InvoiceResponse>>>>
    {
        public BaseQuery Query { get; set; } = new();
    }
}
