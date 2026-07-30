using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.ExpenseTypes.Exceptions;
using Services.Features.Financials.ExpenseTypes.Models;
using Services.Features.Financials.ExpenseTypes.Models.Events;
using Services.Features.Financials.ExpenseTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.ExpenseTypes.UseCases.Commands
{
    public class RemoveExpenseTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        ExpenseTypeDbContext expensetypeDbContext
    )
        : CommandHandler(expensetypeDbContext, mediator),
            IRequestHandler<RemoveExpenseTypeRequest, Result<Response<ExpenseTypeResponse>>>
    {
        private readonly ExpenseTypeDbContext _expensetypeDbContext = expensetypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<ExpenseTypeResponse>>> Handle(
            RemoveExpenseTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentExpenseTypeAsync(req.Id, cancellationToken))
                .BindAsync(currentExpenseType =>
                    RemoveExpenseTypeAsync(currentExpenseType, cancellationToken)
                )
                .MapAsync(currentExpenseType =>
                {
                    return new Response<ExpenseTypeResponse>(null);
                });
        }

        private static Result<RemoveExpenseTypeRequest> ValidateRequest(
            RemoveExpenseTypeRequest request
        )
        {
            return request.Id == default
                ? Result<RemoveExpenseTypeRequest>.Failure(ExpenseTypeErrors.NotFound(request.Id))
                : Result<RemoveExpenseTypeRequest>.Success(request);
        }

        private async Task<Result<ExpenseType>> GetCurrentExpenseTypeAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var expensetype = await _expensetypeDbContext
                .ExpenseTypes.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return expensetype is null
                ? Result<ExpenseType>.Failure(ExpenseTypeErrors.NotFound(id))
                : Result<ExpenseType>.Success(expensetype);
        }

        private async Task<Result<ExpenseType>> RemoveExpenseTypeAsync(
            ExpenseType removeExpenseType,
            CancellationToken cancellationToken
        )
        {
            removeExpenseType.DeletedAt = _dateTimeService.UtcNow;
            removeExpenseType.EditedAt = _dateTimeService.UtcNow;
            removeExpenseType.EditedBy = _authenticatedUserService.UserId;

            removeExpenseType.AddEvent(new ExpenseTypeRemovedEvent(removeExpenseType.Id));

            await ExecuteTransactionAsync(
                () => _expensetypeDbContext.Update(removeExpenseType),
                removeExpenseType.GetEvents(),
                cancellationToken
            );

            return Result<ExpenseType>.Success(removeExpenseType);
        }
    }
}
