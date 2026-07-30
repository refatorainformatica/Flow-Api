using MediatR;
using Services.Features.Financials.Invoices.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Invoices.UseCases.Queries
{
    public class GetByIdInvoiceRequest : IRequest<Result<Response<InvoiceResponse>>>
    {
        public int Id { get; set; }
    }
}
