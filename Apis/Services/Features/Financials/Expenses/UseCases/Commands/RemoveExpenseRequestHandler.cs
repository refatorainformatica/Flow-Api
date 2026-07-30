using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.Expenses.Exceptions;
using Services.Features.Financials.Expenses.Models;
using Services.Features.Financials.Expenses.Models.Events;
using Services.Features.Financials.Expenses.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.Expenses.UseCases.Commands
{
    public class RemoveExpenseRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        ExpenseDbContext expenseDbContext
    )
        : CommandHandler(expenseDbContext, mediator),
            IRequestHandler<RemoveExpenseRequest, Result<Response<ExpenseResponse>>>
    {
        private readonly ExpenseDbContext _expenseDbContext = expenseDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<ExpenseResponse>>> Handle(
            RemoveExpenseRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentExpenseAsync(req.Id, cancellationToken))
                .BindAsync(currentExpense => RemoveExpenseAsync(currentExpense, cancellationToken))
                .MapAsync(currentExpense =>
                {
                    return new Response<ExpenseResponse>(null);
                });
        }

        private static Result<RemoveExpenseRequest> ValidateRequest(RemoveExpenseRequest request)
        {
            return request.Id == default
                ? Result<RemoveExpenseRequest>.Failure(ExpenseErrors.NotFound(request.Id))
                : Result<RemoveExpenseRequest>.Success(request);
        }

        private async Task<Result<Expense>> GetCurrentExpenseAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var expense = await _expenseDbContext
                .Expenses.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return expense is null
                ? Result<Expense>.Failure(ExpenseErrors.NotFound(id))
                : Result<Expense>.Success(expense);
        }

        private async Task<Result<Expense>> RemoveExpenseAsync(
            Expense removeExpense,
            CancellationToken cancellationToken
        )
        {
            removeExpense.DeletedAt = _dateTimeService.UtcNow;
            removeExpense.EditedAt = _dateTimeService.UtcNow;
            removeExpense.EditedBy = _authenticatedUserService.UserId;

            removeExpense.AddEvent(new ExpenseRemovedEvent(removeExpense.Id));

            await ExecuteTransactionAsync(
                () => _expenseDbContext.Update(removeExpense),
                removeExpense.GetEvents(),
                cancellationToken
            );

            return Result<Expense>.Success(removeExpense);
        }
    }
}
