using AutoMapper;
using MediatR;
using Services.Features.Financials.CashFlows.Models;
using Services.Features.Financials.CashFlows.Models.Events;
using Services.Features.Financials.CashFlows.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.CashFlows.UseCases.Commands
{
    public class CreateCashFlowRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        CashFlowDbContext cashflowDbContext
    )
        : CommandHandler(cashflowDbContext, mediator),
            IRequestHandler<CreateCashFlowRequest, Result<Response<CashFlowResponse>>>
    {
        private readonly CashFlowDbContext _cashflowDbContext = cashflowDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<CashFlowResponse>>> Handle(
            CreateCashFlowRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveCashFlowAsync(request, cancellationToken)
                .BindAsync(cashflow => Task.FromResult(GenerateResponse(cashflow)));
        }

        private async Task<Result<CashFlow>> SaveCashFlowAsync(
            CreateCashFlowRequest request,
            CancellationToken cancellationToken
        )
        {
            var newCashFlow = new CashFlow(
                0,
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
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newCashFlow.AddEvent(new CashFlowCreatedEvent(newCashFlow.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _cashflowDbContext.CashFlows.AddAsync(
                        newCashFlow,
                        cancellationToken: cancellationToken
                    );
                },
                newCashFlow.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<CashFlow>.Success(newCashFlow);
        }

        private Result<Response<CashFlowResponse>> GenerateResponse(CashFlow cashflow)
        {
            var cashflowResponse = mapper.Map<CashFlowResponse>(cashflow);
            var response = new Response<CashFlowResponse>(cashflowResponse);

            return Result<Response<CashFlowResponse>>.Success(response);
        }
    }
}
