using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.PaymentStates.Exceptions;
using Services.Features.Financials.PaymentStates.Models;
using Services.Features.Financials.PaymentStates.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.PaymentStates.UseCases.Queries
{
    public class GetByIdPaymentStateRequestHandler(
        IMapper mapper,
        IMediator mediator,
        PaymentStateDbContext paymentstateDbContext
    )
        : CommandHandler(paymentstateDbContext, mediator),
            IRequestHandler<GetByIdPaymentStateRequest, Result<Response<PaymentStateResponse>>>
    {
        private readonly PaymentStateDbContext _paymentstateDbContext = paymentstateDbContext;

        public async Task<Result<Response<PaymentStateResponse>>> Handle(
            GetByIdPaymentStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdPaymentStateAsync(request, cancellationToken)
                .BindAsync(paymentstates => Task.FromResult(GenerateResponse(paymentstates)));
        }

        private async Task<Result<PaymentState>> GetByIdPaymentStateAsync(
            GetByIdPaymentStateRequest request,
            CancellationToken cancellationToken
        )
        {
            var paymentstate = await _paymentstateDbContext
                .PaymentStates.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return paymentstate is null
                ? Result<PaymentState>.Failure(PaymentStateErrors.NotFound(request.Id))
                : Result<PaymentState>.Success(paymentstate);
        }

        private Result<Response<PaymentStateResponse>> GenerateResponse(PaymentState paymentstate)
        {
            var paymentstateResponse = mapper.Map<PaymentStateResponse>(paymentstate);
            var response = new Response<PaymentStateResponse>(paymentstateResponse);
            return Result<Response<PaymentStateResponse>>.Success(response);
        }
    }
}
