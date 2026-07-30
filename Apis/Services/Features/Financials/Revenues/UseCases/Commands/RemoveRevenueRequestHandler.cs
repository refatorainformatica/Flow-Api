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
    public class RemoveRevenueRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        RevenueDbContext revenueDbContext
    )
        : CommandHandler(revenueDbContext, mediator),
            IRequestHandler<RemoveRevenueRequest, Result<Response<RevenueResponse>>>
    {
        private readonly RevenueDbContext _revenueDbContext = revenueDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<RevenueResponse>>> Handle(
            RemoveRevenueRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentRevenueAsync(req.Id, cancellationToken))
                .BindAsync(currentRevenue => RemoveRevenueAsync(currentRevenue, cancellationToken))
                .MapAsync(currentRevenue =>
                {
                    return new Response<RevenueResponse>(null);
                });
        }

        private static Result<RemoveRevenueRequest> ValidateRequest(RemoveRevenueRequest request)
        {
            return request.Id == default
                ? Result<RemoveRevenueRequest>.Failure(RevenueErrors.NotFound(request.Id))
                : Result<RemoveRevenueRequest>.Success(request);
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

        private async Task<Result<Revenue>> RemoveRevenueAsync(
            Revenue removeRevenue,
            CancellationToken cancellationToken
        )
        {
            removeRevenue.DeletedAt = _dateTimeService.UtcNow;
            removeRevenue.EditedAt = _dateTimeService.UtcNow;
            removeRevenue.EditedBy = _authenticatedUserService.UserId;

            removeRevenue.AddEvent(new RevenueRemovedEvent(removeRevenue.Id));

            await ExecuteTransactionAsync(
                () => _revenueDbContext.Update(removeRevenue),
                removeRevenue.GetEvents(),
                cancellationToken
            );

            return Result<Revenue>.Success(removeRevenue);
        }
    }
}
