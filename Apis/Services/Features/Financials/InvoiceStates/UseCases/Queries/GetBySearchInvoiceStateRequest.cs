using MediatR;
using Services.Features.Financials.InvoiceStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.InvoiceStates.UseCases.Queries
{
    public class GetBySearchInvoiceStateRequest
        : IRequest<Result<Response<IEnumerable<InvoiceStateResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
