using MediatR;
using Services.Features.Financials.CashFlows.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.CashFlows.UseCases.Queries
{
    public class GetBySearchCashFlowRequest
        : IRequest<Result<Response<IEnumerable<CashFlowResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
