using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.Expenses.Exceptions;
using Services.Features.Financials.Expenses.Models;
using Services.Features.Financials.Expenses.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Expenses.UseCases.Queries
{
    public class GetByIdExpenseRequestHandler(
        IMapper mapper,
        IMediator mediator,
        ExpenseDbContext expenseDbContext
    )
        : CommandHandler(expenseDbContext, mediator),
            IRequestHandler<GetByIdExpenseRequest, Result<Response<ExpenseResponse>>>
    {
        private readonly ExpenseDbContext _expenseDbContext = expenseDbContext;

        public async Task<Result<Response<ExpenseResponse>>> Handle(
            GetByIdExpenseRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdExpenseAsync(request, cancellationToken)
                .BindAsync(expenses => Task.FromResult(GenerateResponse(expenses)));
        }

        private async Task<Result<Expense>> GetByIdExpenseAsync(
            GetByIdExpenseRequest request,
            CancellationToken cancellationToken
        )
        {
            var expense = await _expenseDbContext
                .Expenses.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return expense is null
                ? Result<Expense>.Failure(ExpenseErrors.NotFound(request.Id))
                : Result<Expense>.Success(expense);
        }

        private Result<Response<ExpenseResponse>> GenerateResponse(Expense expense)
        {
            var expenseResponse = mapper.Map<ExpenseResponse>(expense);
            var response = new Response<ExpenseResponse>(expenseResponse);
            return Result<Response<ExpenseResponse>>.Success(response);
        }
    }
}
