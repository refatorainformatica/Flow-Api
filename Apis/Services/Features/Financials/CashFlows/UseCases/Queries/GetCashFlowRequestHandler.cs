using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.CashFlows.Exceptions;
using Services.Features.Financials.CashFlows.Models;
using Services.Features.Financials.CashFlows.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.CashFlows.UseCases.Queries
{
    public class GetCashFlowRequestHandler(
        IMapper mapper,
        IMediator mediator,
        CashFlowDbContext cashflowDbContext
    )
        : CommandHandler(cashflowDbContext, mediator),
            IRequestHandler<GetCashFlowRequest, Result<Response<IEnumerable<CashFlowResponse>>>>
    {
        private readonly CashFlowDbContext _cashflowDbContext = cashflowDbContext;

        public async Task<Result<Response<IEnumerable<CashFlowResponse>>>> Handle(
            GetCashFlowRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetCashFlowAsync(request)
                .BindAsync(cashflows => Task.FromResult(GenerateResponse(cashflows)));
        }

        private async Task<Result<Pagination<CashFlow>>> GetCashFlowAsync(
            GetCashFlowRequest request
        )
        {
            var cashflows = await Task.Run(
                () =>
                    _cashflowDbContext
                        .CashFlows.AsNoTracking()
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<CashFlow>()
            );

            return !cashflows.Rows.Any()
                ? Result<Pagination<CashFlow>>.Failure(CashFlowErrors.IsEmpty())
                : Result<Pagination<CashFlow>>.Success(cashflows);
        }

        private Result<Response<IEnumerable<CashFlowResponse>>> GenerateResponse(
            Pagination<CashFlow> paginationCashFlow
        )
        {
            var cashflowResponse = mapper.Map<IEnumerable<CashFlowResponse>>(
                paginationCashFlow.Rows
            );
            var response = new Response<IEnumerable<CashFlowResponse>>(
                cashflowResponse,
                paginationCashFlow.Offset,
                paginationCashFlow.Limit,
                paginationCashFlow.PageCount,
                paginationCashFlow.RowCount
            );
            return Result<Response<IEnumerable<CashFlowResponse>>>.Success(response);
        }
    }
}
