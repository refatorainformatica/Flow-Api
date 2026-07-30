using MediatR;
using Services.Features.Financials.InvoiceTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.InvoiceTypes.UseCases.Queries
{
    public class GetInvoiceTypeRequest
        : IRequest<Result<Response<IEnumerable<InvoiceTypeResponse>>>>
    {
        public BaseQuery Query { get; set; } = new();
    }
}
