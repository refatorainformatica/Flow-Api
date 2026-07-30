using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.Revenues.Exceptions;
using Services.Features.Financials.Revenues.Models;
using Services.Features.Financials.Revenues.Models.Events;
using Services.Features.Financials.Revenues.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.Revenues.UseCases.Commands
{
    public class EditRevenueRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        RevenueDbContext revenueDbContext
    )
        : CommandHandler(revenueDbContext, mediator),
            IRequestHandler<EditRevenueRequest, Result<Response<RevenueResponse>>>
    {
        private readonly RevenueDbContext _revenueDbContext = revenueDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<RevenueResponse>>> Handle(
            EditRevenueRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentRevenueAsync(req.Id, cancellationToken))
                .BindAsync(currentRevenue =>
                    EditAndSaveRevenueAsync(currentRevenue, request, cancellationToken)
                )
                .MapAsync(currentRevenue =>
                {
                    return new Response<RevenueResponse>(null);
                });
        }

        private static Result<EditRevenueRequest> ValidateRequest(EditRevenueRequest request)
        {
            return request.Id != request.RequestId
                ? Result<EditRevenueRequest>.Failure(
                    RevenueErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditRevenueRequest>.Success(request);
        }

        private async Task<Result<Revenue>> GetCurrentRevenueAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var revenue = await _revenueDbContext
                .Revenues.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return revenue is null
                ? Result<Revenue>.Failure(RevenueErrors.NotFound(id))
                : Result<Revenue>.Success(revenue);
        }

        private async Task<Result<Revenue>> EditAndSaveRevenueAsync(
            Revenue currentRevenue,
            EditRevenueRequest request,
            CancellationToken cancellationToken
        )
        {
            var editRevenue = new Revenue(
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
                request.RevenueTypeId,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentRevenue.CreatedAt.GetValueOrDefault(),
                currentRevenue.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editRevenue.AddEvent(new RevenueEditedEvent(editRevenue.Id));

            await ExecuteTransactionAsync(
                () => _revenueDbContext.Revenues.Update(editRevenue),
                editRevenue.GetEvents(),
                cancellationToken
            );

            return Result<Revenue>.Success(editRevenue);
        }
    }
}
