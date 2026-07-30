using MediatR;
using Services.Features.Financials.RevenueTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.RevenueTypes.UseCases.Queries
{
    public class GetRevenueTypeRequest
        : IRequest<Result<Response<IEnumerable<RevenueTypeResponse>>>>
    {
        public BaseQuery Query { get; set; } = new();
    }
}
