using MediatR;
using Services.Features.Financials.InvoiceStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.InvoiceStates.UseCases.Commands
{
    public class EditInvoiceStateRequest
        : InvoiceStateRequest,
            IRequest<Result<Response<InvoiceStateResponse>>>
    {
        public int RequestId { get; set; }
    }
}
