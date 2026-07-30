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
    public class EditPaymentStateRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        PaymentStateDbContext paymentstateDbContext
    )
        : CommandHandler(paymentstateDbContext, mediator),
            IRequestHandler<EditPaymentStateRequest, Result<Response<PaymentStateResponse>>>
    {
        private readonly PaymentStateDbContext _paymentstateDbContext = paymentstateDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<PaymentStateResponse>>> Handle(
            EditPaymentStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentPaymentStateAsync(req.Id, cancellationToken))
                .BindAsync(currentPaymentState =>
                    EditAndSavePaymentStateAsync(currentPaymentState, request, cancellationToken)
                )
                .MapAsync(currentPaymentState =>
                {
                    return new Response<PaymentStateResponse>(null);
                });
        }

        private static Result<EditPaymentStateRequest> ValidateRequest(
            EditPaymentStateRequest request
        )
        {
            return request.Id != request.RequestId
                ? Result<EditPaymentStateRequest>.Failure(
                    PaymentStateErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditPaymentStateRequest>.Success(request);
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

        private async Task<Result<PaymentState>> EditAndSavePaymentStateAsync(
            PaymentState currentPaymentState,
            EditPaymentStateRequest request,
            CancellationToken cancellationToken
        )
        {
            var editPaymentState = new PaymentState(
                request.Id,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentPaymentState.CreatedAt.GetValueOrDefault(),
                currentPaymentState.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editPaymentState.AddEvent(new PaymentStateEditedEvent(editPaymentState.Id));

            await ExecuteTransactionAsync(
                () => _paymentstateDbContext.PaymentStates.Update(editPaymentState),
                editPaymentState.GetEvents(),
                cancellationToken
            );

            return Result<PaymentState>.Success(editPaymentState);
        }
    }
}
