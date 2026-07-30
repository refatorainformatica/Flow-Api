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
    public class EditCashFlowRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        CashFlowDbContext cashflowDbContext
    )
        : CommandHandler(cashflowDbContext, mediator),
            IRequestHandler<EditCashFlowRequest, Result<Response<CashFlowResponse>>>
    {
        private readonly CashFlowDbContext _cashflowDbContext = cashflowDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<CashFlowResponse>>> Handle(
            EditCashFlowRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentCashFlowAsync(req.Id, cancellationToken))
                .BindAsync(currentCashFlow =>
                    EditAndSaveCashFlowAsync(currentCashFlow, request, cancellationToken)
                )
                .MapAsync(currentCashFlow =>
                {
                    return new Response<CashFlowResponse>(null);
                });
        }

        private static Result<EditCashFlowRequest> ValidateRequest(EditCashFlowRequest request)
        {
            return request.Id != request.RequestId
                ? Result<EditCashFlowRequest>.Failure(
                    CashFlowErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditCashFlowRequest>.Success(request);
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

        private async Task<Result<CashFlow>> EditAndSaveCashFlowAsync(
            CashFlow currentCashFlow,
            EditCashFlowRequest request,
            CancellationToken cancellationToken
        )
        {
            var editCashFlow = new CashFlow(
                request.Id,
                request.YearExercise,
                request.MonthExercise,
                request.MovementTypeId,
                request.FinancialMovementDate,
                request.FinancialMovementValue,
                request.BalanceValue,
                request.ExpenseId,
                request.RevenueId,
                request.CurrencyTypeId,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentCashFlow.CreatedAt.GetValueOrDefault(),
                currentCashFlow.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editCashFlow.AddEvent(new CashFlowEditedEvent(editCashFlow.Id));

            await ExecuteTransactionAsync(
                () => _cashflowDbContext.CashFlows.Update(editCashFlow),
                editCashFlow.GetEvents(),
                cancellationToken
            );

            return Result<CashFlow>.Success(editCashFlow);
        }
    }
}
