using MediatR;
using Services.Features.Financials.CostCenters.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.CostCenters.UseCases.Commands
{
    public class CreateCostCenterRequest
        : CostCenterRequest,
            IRequest<Result<Response<CostCenterResponse>>> { }
}
