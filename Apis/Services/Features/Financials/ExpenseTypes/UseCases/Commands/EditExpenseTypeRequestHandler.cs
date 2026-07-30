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
    public class EditExpenseTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        ExpenseTypeDbContext expensetypeDbContext
    )
        : CommandHandler(expensetypeDbContext, mediator),
            IRequestHandler<EditExpenseTypeRequest, Result<Response<ExpenseTypeResponse>>>
    {
        private readonly ExpenseTypeDbContext _expensetypeDbContext = expensetypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<ExpenseTypeResponse>>> Handle(
            EditExpenseTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentExpenseTypeAsync(req.Id, cancellationToken))
                .BindAsync(currentExpenseType =>
                    EditAndSaveExpenseTypeAsync(currentExpenseType, request, cancellationToken)
                )
                .MapAsync(currentExpenseType =>
                {
                    return new Response<ExpenseTypeResponse>(null);
                });
        }

        private static Result<EditExpenseTypeRequest> ValidateRequest(
            EditExpenseTypeRequest request
        )
        {
            return request.Id != request.RequestId
                ? Result<EditExpenseTypeRequest>.Failure(
                    ExpenseTypeErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditExpenseTypeRequest>.Success(request);
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

        private async Task<Result<ExpenseType>> EditAndSaveExpenseTypeAsync(
            ExpenseType currentExpenseType,
            EditExpenseTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var editExpenseType = new ExpenseType(
                request.Id,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentExpenseType.CreatedAt.GetValueOrDefault(),
                currentExpenseType.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editExpenseType.AddEvent(new ExpenseTypeEditedEvent(editExpenseType.Id));

            await ExecuteTransactionAsync(
                () => _expensetypeDbContext.ExpenseTypes.Update(editExpenseType),
                editExpenseType.GetEvents(),
                cancellationToken
            );

            return Result<ExpenseType>.Success(editExpenseType);
        }
    }
}
