using MediatR;
using Services.Features.Financials.InvoiceTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.InvoiceTypes.UseCases.Commands
{
    public class RemoveInvoiceTypeRequest : IRequest<Result<Response<InvoiceTypeResponse>>>
    {
        public int Id { get; set; }
    }
}
