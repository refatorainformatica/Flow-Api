using AutoMapper;
using MediatR;
using Services.Features.Financials.ExpenseTypes.Models;
using Services.Features.Financials.ExpenseTypes.Models.Events;
using Services.Features.Financials.ExpenseTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.ExpenseTypes.UseCases.Commands
{
    public class CreateExpenseTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        ExpenseTypeDbContext expensetypeDbContext
    )
        : CommandHandler(expensetypeDbContext, mediator),
            IRequestHandler<CreateExpenseTypeRequest, Result<Response<ExpenseTypeResponse>>>
    {
        private readonly ExpenseTypeDbContext _expensetypeDbContext = expensetypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<ExpenseTypeResponse>>> Handle(
            CreateExpenseTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveExpenseTypeAsync(request, cancellationToken)
                .BindAsync(expensetype => Task.FromResult(GenerateResponse(expensetype)));
        }

        private async Task<Result<ExpenseType>> SaveExpenseTypeAsync(
            CreateExpenseTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var newExpenseType = new ExpenseType(
                0,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newExpenseType.AddEvent(new ExpenseTypeCreatedEvent(newExpenseType.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _expensetypeDbContext.ExpenseTypes.AddAsync(
                        newExpenseType,
                        cancellationToken: cancellationToken
                    );
                },
                newExpenseType.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<ExpenseType>.Success(newExpenseType);
        }

        private Result<Response<ExpenseTypeResponse>> GenerateResponse(ExpenseType expensetype)
        {
            var expensetypeResponse = mapper.Map<ExpenseTypeResponse>(expensetype);
            var response = new Response<ExpenseTypeResponse>(expensetypeResponse);

            return Result<Response<ExpenseTypeResponse>>.Success(response);
        }
    }
}
