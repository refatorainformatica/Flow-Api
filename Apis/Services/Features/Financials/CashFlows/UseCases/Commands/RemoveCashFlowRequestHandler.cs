using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.CashFlows.Exceptions;
using Services.Features.Financials.CashFlows.Models;
using Services.Features.Financials.CashFlows.Models.Events;
using Services.Features.Financials.CashFlows.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.CashFlows.UseCases.Commands
{
    public class RemoveCashFlowRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        CashFlowDbContext cashflowDbContext
    )
        : CommandHandler(cashflowDbContext, mediator),
            IRequestHandler<RemoveCashFlowRequest, Result<Response<CashFlowResponse>>>
    {
        private readonly CashFlowDbContext _cashflowDbContext = cashflowDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<CashFlowResponse>>> Handle(
            RemoveCashFlowRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentCashFlowAsync(req.Id, cancellationToken))
                .BindAsync(currentCashFlow =>
                    RemoveCashFlowAsync(currentCashFlow, cancellationToken)
                )
                .MapAsync(currentCashFlow =>
                {
                    return new Response<CashFlowResponse>(null);
                });
        }

        private static Result<RemoveCashFlowRequest> ValidateRequest(RemoveCashFlowRequest request)
        {
            return request.Id == default
                ? Result<RemoveCashFlowRequest>.Failure(CashFlowErrors.NotFound(request.Id))
                : Result<RemoveCashFlowRequest>.Success(request);
        }

        private async Task<Result<CashFlow>> GetCurrentCashFlowAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var cashflow = await _cashflowDbContext
                .CashFlows.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return cashflow is null
                ? Result<CashFlow>.Failure(CashFlowErrors.NotFound(id))
                : Result<CashFlow>.Success(cashflow);
        }

        private async Task<Result<CashFlow>> RemoveCashFlowAsync(
            CashFlow removeCashFlow,
            CancellationToken cancellationToken
        )
        {
            removeCashFlow.DeletedAt = _dateTimeService.UtcNow;
            removeCashFlow.EditedAt = _dateTimeService.UtcNow;
            removeCashFlow.EditedBy = _authenticatedUserService.UserId;

            removeCashFlow.AddEvent(new CashFlowRemovedEvent(removeCashFlow.Id));

            await ExecuteTransactionAsync(
                () => _cashflowDbContext.Update(removeCashFlow),
                removeCashFlow.GetEvents(),
                cancellationToken
            );

            return Result<CashFlow>.Success(removeCashFlow);
        }
    }
}
