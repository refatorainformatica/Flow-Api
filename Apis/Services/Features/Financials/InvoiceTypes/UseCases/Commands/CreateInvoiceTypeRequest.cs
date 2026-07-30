using MediatR;
using Services.Features.Financials.InvoiceTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.InvoiceTypes.UseCases.Commands
{
    public class CreateInvoiceTypeRequest
        : InvoiceTypeRequest,
            IRequest<Result<Response<InvoiceTypeResponse>>> { }
}
