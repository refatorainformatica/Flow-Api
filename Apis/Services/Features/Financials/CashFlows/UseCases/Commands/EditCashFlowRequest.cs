using MediatR;
using Services.Features.Financials.CashFlows.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.CashFlows.UseCases.Commands
{
    public class EditCashFlowRequest : CashFlowRequest, IRequest<Result<Response<CashFlowResponse>>>
    {
        public int RequestId { get; set; }
    }
}
