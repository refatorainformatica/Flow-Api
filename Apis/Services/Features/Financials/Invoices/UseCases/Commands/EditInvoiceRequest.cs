using MediatR;
using Services.Features.Financials.Invoices.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Invoices.UseCases.Commands
{
    public class EditInvoiceRequest : InvoiceRequest, IRequest<Result<Response<InvoiceResponse>>>
    {
        public int RequestId { get; set; }
    }
}
