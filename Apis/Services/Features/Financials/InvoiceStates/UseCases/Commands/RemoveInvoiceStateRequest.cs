using MediatR;
using Services.Features.Financials.InvoiceStates.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.InvoiceStates.UseCases.Commands
{
    public class RemoveInvoiceStateRequest : IRequest<Result<Response<InvoiceStateResponse>>>
    {
        public int Id { get; set; }
    }
}
