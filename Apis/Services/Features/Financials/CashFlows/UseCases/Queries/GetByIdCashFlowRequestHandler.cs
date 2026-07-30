using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.CashFlows.Exceptions;
using Services.Features.Financials.CashFlows.Models;
using Services.Features.Financials.CashFlows.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.CashFlows.UseCases.Queries
{
    public class GetByIdCashFlowRequestHandler(
        IMapper mapper,
        IMediator mediator,
        CashFlowDbContext cashflowDbContext
    )
        : CommandHandler(cashflowDbContext, mediator),
            IRequestHandler<GetByIdCashFlowRequest, Result<Response<CashFlowResponse>>>
    {
        private readonly CashFlowDbContext _cashflowDbContext = cashflowDbContext;

        public async Task<Result<Response<CashFlowResponse>>> Handle(
            GetByIdCashFlowRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdCashFlowAsync(request, cancellationToken)
                .BindAsync(cashflows => Task.FromResult(GenerateResponse(cashflows)));
        }

        private async Task<Result<CashFlow>> GetByIdCashFlowAsync(
            GetByIdCashFlowRequest request,
            CancellationToken cancellationToken
        )
        {
            var cashflow = await _cashflowDbContext
                .CashFlows.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return cashflow is null
                ? Result<CashFlow>.Failure(CashFlowErrors.NotFound(request.Id))
                : Result<CashFlow>.Success(cashflow);
        }

        private Result<Response<CashFlowResponse>> GenerateResponse(CashFlow cashflow)
        {
            var cashflowResponse = mapper.Map<CashFlowResponse>(cashflow);
            var response = new Response<CashFlowResponse>(cashflowResponse);
            return Result<Response<CashFlowResponse>>.Success(response);
        }
    }
}
