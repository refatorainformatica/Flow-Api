using MediatR;
using Services.Features.Financials.InvoiceTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.InvoiceTypes.UseCases.Commands
{
    public class EditInvoiceTypeRequest
        : InvoiceTypeRequest,
            IRequest<Result<Response<InvoiceTypeResponse>>>
    {
        public int RequestId { get; set; }
    }
}
