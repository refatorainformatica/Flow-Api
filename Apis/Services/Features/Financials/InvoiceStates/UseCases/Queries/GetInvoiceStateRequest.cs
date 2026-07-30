using MediatR;
using Services.Features.Financials.InvoiceStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.InvoiceStates.UseCases.Queries
{
    public class GetInvoiceStateRequest
        : IRequest<Result<Response<IEnumerable<InvoiceStateResponse>>>>
    {
        public BaseQuery Query { get; set; } = new();
    }
}
