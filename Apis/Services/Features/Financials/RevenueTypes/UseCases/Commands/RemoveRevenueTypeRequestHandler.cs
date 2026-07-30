using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.RevenueTypes.Exceptions;
using Services.Features.Financials.RevenueTypes.Models;
using Services.Features.Financials.RevenueTypes.Models.Events;
using Services.Features.Financials.RevenueTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.RevenueTypes.UseCases.Commands
{
    public class RemoveRevenueTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        RevenueTypeDbContext revenuetypeDbContext
    )
        : CommandHandler(revenuetypeDbContext, mediator),
            IRequestHandler<RemoveRevenueTypeRequest, Result<Response<RevenueTypeResponse>>>
    {
        private readonly RevenueTypeDbContext _revenuetypeDbContext = revenuetypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<RevenueTypeResponse>>> Handle(
            RemoveRevenueTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentRevenueTypeAsync(req.Id, cancellationToken))
                .BindAsync(currentRevenueType =>
                    RemoveRevenueTypeAsync(currentRevenueType, cancellationToken)
                )
                .MapAsync(currentRevenueType =>
                {
                    return new Response<RevenueTypeResponse>(null);
                });
        }

        private static Result<RemoveRevenueTypeRequest> ValidateRequest(
            RemoveRevenueTypeRequest request
        )
        {
            return request.Id == default
                ? Result<RemoveRevenueTypeRequest>.Failure(RevenueTypeErrors.NotFound(request.Id))
                : Result<RemoveRevenueTypeRequest>.Success(request);
        }

        private async Task<Result<RevenueType>> GetCurrentRevenueTypeAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var revenuetype = await _revenuetypeDbContext
                .RevenueTypes.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return revenuetype is null
                ? Result<RevenueType>.Failure(RevenueTypeErrors.NotFound(id))
                : Result<RevenueType>.Success(revenuetype);
        }

        private async Task<Result<RevenueType>> RemoveRevenueTypeAsync(
            RevenueType removeRevenueType,
            CancellationToken cancellationToken
        )
        {
            removeRevenueType.DeletedAt = _dateTimeService.UtcNow;
            removeRevenueType.EditedAt = _dateTimeService.UtcNow;
            removeRevenueType.EditedBy = _authenticatedUserService.UserId;

            removeRevenueType.AddEvent(new RevenueTypeRemovedEvent(removeRevenueType.Id));

            await ExecuteTransactionAsync(
                () => _revenuetypeDbContext.Update(removeRevenueType),
                removeRevenueType.GetEvents(),
                cancellationToken
            );

            return Result<RevenueType>.Success(removeRevenueType);
        }
    }
}
