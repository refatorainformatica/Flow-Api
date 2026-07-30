using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.Expenses.Exceptions;
using Services.Features.Financials.Expenses.Models;
using Services.Features.Financials.Expenses.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.Expenses.UseCases.Queries
{
    public class GetBySearchExpenseRequestHandler(
        IMapper mapper,
        IMediator mediator,
        ExpenseDbContext expenseDbContext
    )
        : CommandHandler(expenseDbContext, mediator),
            IRequestHandler<
                GetBySearchExpenseRequest,
                Result<Response<IEnumerable<ExpenseResponse>>>
            >
    {
        private readonly ExpenseDbContext _expenseDbContext = expenseDbContext;

        public async Task<Result<Response<IEnumerable<ExpenseResponse>>>> Handle(
            GetBySearchExpenseRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetBySearchExpenseAsync(request)
                .BindAsync(expenses => Task.FromResult(GenerateResponse(expenses)));
        }

        private async Task<Result<Pagination<Expense>>> GetBySearchExpenseAsync(
            GetBySearchExpenseRequest request
        )
        {
            var expenses = await Task.Run(
                () =>
                    _expenseDbContext
                        .Expenses.AsNoTracking()
                        .SearchBy(request.Query.SearchText)
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<Expense>()
            );

            return !expenses.Rows.Any()
                ? Result<Pagination<Expense>>.Failure(
                    ExpenseErrors.NotFound(request.Query.SearchText)
                )
                : Result<Pagination<Expense>>.Success(expenses);
        }

        private Result<Response<IEnumerable<ExpenseResponse>>> GenerateResponse(
            Pagination<Expense> paginationExpense
        )
        {
            var expenseResponse = mapper.Map<IEnumerable<ExpenseResponse>>(paginationExpense.Rows);
            var response = new Response<IEnumerable<ExpenseResponse>>(
                expenseResponse,
                paginationExpense.Offset,
                paginationExpense.Limit,
                paginationExpense.PageCount,
                paginationExpense.RowCount
            );
            return Result<Response<IEnumerable<ExpenseResponse>>>.Success(response);
        }
    }
}
