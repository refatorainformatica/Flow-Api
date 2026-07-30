using MediatR;
using Services.Features.Financials.CostCenters.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.CostCenters.UseCases.Queries
{
    public class GetByIdCostCenterRequest : IRequest<Result<Response<CostCenterResponse>>>
    {
        public int Id { get; set; }
    }
}
