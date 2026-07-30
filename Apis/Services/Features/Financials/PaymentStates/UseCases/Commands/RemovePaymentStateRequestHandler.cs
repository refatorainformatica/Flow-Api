using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.PaymentStates.Exceptions;
using Services.Features.Financials.PaymentStates.Models;
using Services.Features.Financials.PaymentStates.Models.Events;
using Services.Features.Financials.PaymentStates.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.PaymentStates.UseCases.Commands
{
    public class RemovePaymentStateRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        PaymentStateDbContext paymentstateDbContext
    )
        : CommandHandler(paymentstateDbContext, mediator),
            IRequestHandler<RemovePaymentStateRequest, Result<Response<PaymentStateResponse>>>
    {
        private readonly PaymentStateDbContext _paymentstateDbContext = paymentstateDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<PaymentStateResponse>>> Handle(
            RemovePaymentStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentPaymentStateAsync(req.Id, cancellationToken))
                .BindAsync(currentPaymentState =>
                    RemovePaymentStateAsync(currentPaymentState, cancellationToken)
                )
                .MapAsync(currentPaymentState =>
                {
                    return new Response<PaymentStateResponse>(null);
                });
        }

        private static Result<RemovePaymentStateRequest> ValidateRequest(
            RemovePaymentStateRequest request
        )
        {
            return request.Id == default
                ? Result<RemovePaymentStateRequest>.Failure(PaymentStateErrors.NotFound(request.Id))
                : Result<RemovePaymentStateRequest>.Success(request);
        }

        private async Task<Result<PaymentState>> GetCurrentPaymentStateAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var paymentstate = await _paymentstateDbContext
                .PaymentStates.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return paymentstate is null
                ? Result<PaymentState>.Failure(PaymentStateErrors.NotFound(id))
                : Result<PaymentState>.Success(paymentstate);
        }

        private async Task<Result<PaymentState>> RemovePaymentStateAsync(
            PaymentState removePaymentState,
            CancellationToken cancellationToken
        )
        {
            removePaymentState.DeletedAt = _dateTimeService.UtcNow;
            removePaymentState.EditedAt = _dateTimeService.UtcNow;
            removePaymentState.EditedBy = _authenticatedUserService.UserId;

            removePaymentState.AddEvent(new PaymentStateRemovedEvent(removePaymentState.Id));

            await ExecuteTransactionAsync(
                () => _paymentstateDbContext.Update(removePaymentState),
                removePaymentState.GetEvents(),
                cancellationToken
            );

            return Result<PaymentState>.Success(removePaymentState);
        }
    }
}
