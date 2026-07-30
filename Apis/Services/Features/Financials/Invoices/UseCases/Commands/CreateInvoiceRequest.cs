using MediatR;
using Services.Features.Financials.Invoices.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Invoices.UseCases.Commands
{
    public class CreateInvoiceRequest
        : InvoiceRequest,
            IRequest<Result<Response<InvoiceResponse>>> { }
}
