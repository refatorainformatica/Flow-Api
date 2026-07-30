using MediatR;
using Services.Features.Financials.Revenues.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Revenues.UseCases.Commands
{
    public class EditRevenueRequest : RevenueRequest, IRequest<Result<Response<RevenueResponse>>>
    {
        public int RequestId { get; set; }
    }
}
