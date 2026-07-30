using AutoMapper;
using MediatR;
using Services.Features.Financials.Expenses.Models;
using Services.Features.Financials.Expenses.Models.Events;
using Services.Features.Financials.Expenses.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.Expenses.UseCases.Commands
{
    public class CreateExpenseRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        ExpenseDbContext expenseDbContext
    )
        : CommandHandler(expenseDbContext, mediator),
            IRequestHandler<CreateExpenseRequest, Result<Response<ExpenseResponse>>>
    {
        private readonly ExpenseDbContext _expenseDbContext = expenseDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<ExpenseResponse>>> Handle(
            CreateExpenseRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveExpenseAsync(request, cancellationToken)
                .BindAsync(expense => Task.FromResult(GenerateResponse(expense)));
        }

        private async Task<Result<Expense>> SaveExpenseAsync(
            CreateExpenseRequest request,
            CancellationToken cancellationToken
        )
        {
            var newExpense = new Expense(
                0,
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
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newExpense.AddEvent(new ExpenseCreatedEvent(newExpense.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _expenseDbContext.Expenses.AddAsync(
                        newExpense,
                        cancellationToken: cancellationToken
                    );
                },
                newExpense.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<Expense>.Success(newExpense);
        }

        private Result<Response<ExpenseResponse>> GenerateResponse(Expense expense)
        {
            var expenseResponse = mapper.Map<ExpenseResponse>(expense);
            var response = new Response<ExpenseResponse>(expenseResponse);

            return Result<Response<ExpenseResponse>>.Success(response);
        }
    }
}
