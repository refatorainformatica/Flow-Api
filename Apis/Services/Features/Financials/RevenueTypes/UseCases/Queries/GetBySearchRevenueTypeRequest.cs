using MediatR;
using Services.Features.Financials.RevenueTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.RevenueTypes.UseCases.Queries
{
    public class GetBySearchRevenueTypeRequest
        : IRequest<Result<Response<IEnumerable<RevenueTypeResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
