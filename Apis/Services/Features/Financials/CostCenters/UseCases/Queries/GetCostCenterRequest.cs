using MediatR;
using Services.Features.Financials.CostCenters.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.CostCenters.UseCases.Queries
{
    public class GetCostCenterRequest : IRequest<Result<Response<IEnumerable<CostCenterResponse>>>>
    {
        public BaseQuery Query { get; set; } = new();
    }
}
