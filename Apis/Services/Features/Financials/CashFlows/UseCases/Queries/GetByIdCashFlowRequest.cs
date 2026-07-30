using MediatR;
using Services.Features.Financials.CashFlows.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.CashFlows.UseCases.Queries
{
    public class GetByIdCashFlowRequest : IRequest<Result<Response<CashFlowResponse>>>
    {
        public int Id { get; set; }
    }
}
