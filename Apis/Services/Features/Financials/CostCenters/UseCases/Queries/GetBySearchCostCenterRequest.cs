using MediatR;
using Services.Features.Financials.CostCenters.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.CostCenters.UseCases.Queries
{
    public class GetBySearchCostCenterRequest
        : IRequest<Result<Response<IEnumerable<CostCenterResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
