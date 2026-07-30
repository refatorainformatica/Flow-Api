using MediatR;
using Services.Features.Financials.CashFlows.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.CashFlows.UseCases.Queries
{
    public class GetCashFlowRequest : IRequest<Result<Response<IEnumerable<CashFlowResponse>>>>
    {
        public BaseQuery Query { get; set; } = new();
    }
}
