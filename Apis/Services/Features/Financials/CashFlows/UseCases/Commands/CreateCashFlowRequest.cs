using MediatR;
using Services.Features.Financials.CashFlows.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.CashFlows.UseCases.Commands
{
    public class CreateCashFlowRequest
        : CashFlowRequest,
            IRequest<Result<Response<CashFlowResponse>>> { }
}
