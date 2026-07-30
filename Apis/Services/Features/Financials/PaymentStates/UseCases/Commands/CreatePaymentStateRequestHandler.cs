using AutoMapper;
using MediatR;
using Services.Features.Financials.PaymentStates.Models;
using Services.Features.Financials.PaymentStates.Models.Events;
using Services.Features.Financials.PaymentStates.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.PaymentStates.UseCases.Commands
{
    public class CreatePaymentStateRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        PaymentStateDbContext paymentstateDbContext
    )
        : CommandHandler(paymentstateDbContext, mediator),
            IRequestHandler<CreatePaymentStateRequest, Result<Response<PaymentStateResponse>>>
    {
        private readonly PaymentStateDbContext _paymentstateDbContext = paymentstateDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<PaymentStateResponse>>> Handle(
            CreatePaymentStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SavePaymentStateAsync(request, cancellationToken)
                .BindAsync(paymentstate => Task.FromResult(GenerateResponse(paymentstate)));
        }

        private async Task<Result<PaymentState>> SavePaymentStateAsync(
            CreatePaymentStateRequest request,
            CancellationToken cancellationToken
        )
        {
            var newPaymentState = new PaymentState(
                0,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newPaymentState.AddEvent(new PaymentStateCreatedEvent(newPaymentState.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _paymentstateDbContext.PaymentStates.AddAsync(
                        newPaymentState,
                        cancellationToken: cancellationToken
                    );
                },
                newPaymentState.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<PaymentState>.Success(newPaymentState);
        }

        private Result<Response<PaymentStateResponse>> GenerateResponse(PaymentState paymentstate)
        {
            var paymentstateResponse = mapper.Map<PaymentStateResponse>(paymentstate);
            var response = new Response<PaymentStateResponse>(paymentstateResponse);

            return Result<Response<PaymentStateResponse>>.Success(response);
        }
    }
}
