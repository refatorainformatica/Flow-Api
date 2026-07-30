using MediatR;
using Services.Features.Financials.InvoiceStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.InvoiceStates.UseCases.Queries
{
    public class GetByIdInvoiceStateRequest : IRequest<Result<Response<InvoiceStateResponse>>>
    {
        public int Id { get; set; }
    }
}
