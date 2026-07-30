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
    public class EditExpenseRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        ExpenseDbContext expenseDbContext
    )
        : CommandHandler(expenseDbContext, mediator),
            IRequestHandler<EditExpenseRequest, Result<Response<ExpenseResponse>>>
    {
        private readonly ExpenseDbContext _expenseDbContext = expenseDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<ExpenseResponse>>> Handle(
            EditExpenseRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentExpenseAsync(req.Id, cancellationToken))
                .BindAsync(currentExpense =>
                    EditAndSaveExpenseAsync(currentExpense, request, cancellationToken)
                )
                .MapAsync(currentExpense =>
                {
                    return new Response<ExpenseResponse>(null);
                });
        }

        private static Result<EditExpenseRequest> ValidateRequest(EditExpenseRequest request)
        {
            return request.Id != request.RequestId
                ? Result<EditExpenseRequest>.Failure(
                    ExpenseErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditExpenseRequest>.Success(request);
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

        private async Task<Result<Expense>> EditAndSaveExpenseAsync(
            Expense currentExpense,
            EditExpenseRequest request,
            CancellationToken cancellationToken
        )
        {
            var editExpense = new Expense(
                request.Id,
                request.InvoiceId,
                request.DateOfIssue,
                request.DateOfDue,
                request.DateOfPayment,
                request.InstallmentNumber,
                request.TotalNumberOfInstallments,
                request.PaymentValue,
                request.PaymentDiscountValue,
                request.TotalPaymentValue,
                request.BarCode,
                request.Observation,
                request.CostCenterId,
                request.PaymentStateId,
                request.ExpenseTypeId,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentExpense.CreatedAt.GetValueOrDefault(),
                currentExpense.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editExpense.AddEvent(new ExpenseEditedEvent(editExpense.Id));

            await ExecuteTransactionAsync(
                () => _expenseDbContext.Expenses.Update(editExpense),
                editExpense.GetEvents(),
                cancellationToken
            );

            return Result<Expense>.Success(editExpense);
        }
    }
}
