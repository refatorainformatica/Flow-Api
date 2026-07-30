using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.PaymentStates.Exceptions;
using Services.Features.Financials.PaymentStates.Models;
using Services.Features.Financials.PaymentStates.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.PaymentStates.UseCases.Queries
{
    public class GetBySearchPaymentStateRequestHandler(
        IMapper mapper,
        IMediator mediator,
        PaymentStateDbContext paymentstateDbContext
    )
        : CommandHandler(paymentstateDbContext, mediator),
            IRequestHandler<
                GetBySearchPaymentStateRequest,
                Result<Response<IEnumerable<PaymentStateResponse>>>
            >
    {
        private readonly PaymentStateDbContext _paymentstateDbContext = paymentstateDbContext;

        public async Task<Result<Response<IEnumerable<PaymentStateResponse>>>> Handle(
            GetBySearchPaymentStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetBySearchPaymentStateAsync(request)
                .BindAsync(paymentstates => Task.FromResult(GenerateResponse(paymentstates)));
        }

        private async Task<Result<Pagination<PaymentState>>> GetBySearchPaymentStateAsync(
            GetBySearchPaymentStateRequest request
        )
        {
            var paymentstates = await Task.Run(
                () =>
                    _paymentstateDbContext
                        .PaymentStates.AsNoTracking()
                        .SearchBy(request.Query.SearchText)
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<PaymentState>()
            );

            return !paymentstates.Rows.Any()
                ? Result<Pagination<PaymentState>>.Failure(
                    PaymentStateErrors.NotFound(request.Query.SearchText)
                )
                : Result<Pagination<PaymentState>>.Success(paymentstates);
        }

        private Result<Response<IEnumerable<PaymentStateResponse>>> GenerateResponse(
            Pagination<PaymentState> paginationPaymentState
        )
        {
            var paymentstateResponse = mapper.Map<IEnumerable<PaymentStateResponse>>(
                paginationPaymentState.Rows
            );
            var response = new Response<IEnumerable<PaymentStateResponse>>(
                paymentstateResponse,
                paginationPaymentState.Offset,
                paginationPaymentState.Limit,
                paginationPaymentState.PageCount,
                paginationPaymentState.RowCount
            );
            return Result<Response<IEnumerable<PaymentStateResponse>>>.Success(response);
        }
    }
}
