using MediatR;
using Services.Features.Financials.Revenues.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Revenues.UseCases.Queries
{
    public class GetBySearchRevenueRequest
        : IRequest<Result<Response<IEnumerable<RevenueResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
