using MediatR;
using Services.Features.Financials.InvoiceTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.InvoiceTypes.UseCases.Queries
{
    public class GetBySearchInvoiceTypeRequest
        : IRequest<Result<Response<IEnumerable<InvoiceTypeResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
