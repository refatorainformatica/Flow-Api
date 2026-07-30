using MediatR;
using Services.Features.Financials.Revenues.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Revenues.UseCases.Queries
{
    public class GetRevenueRequest : IRequest<Result<Response<IEnumerable<RevenueResponse>>>>
    {
        public BaseQuery Query { get; set; } = new();
    }
}
