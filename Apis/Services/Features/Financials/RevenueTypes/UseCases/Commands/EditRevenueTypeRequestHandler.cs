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
    public class EditRevenueTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        RevenueTypeDbContext revenuetypeDbContext
    )
        : CommandHandler(revenuetypeDbContext, mediator),
            IRequestHandler<EditRevenueTypeRequest, Result<Response<RevenueTypeResponse>>>
    {
        private readonly RevenueTypeDbContext _revenuetypeDbContext = revenuetypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<RevenueTypeResponse>>> Handle(
            EditRevenueTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentRevenueTypeAsync(req.Id, cancellationToken))
                .BindAsync(currentRevenueType =>
                    EditAndSaveRevenueTypeAsync(currentRevenueType, request, cancellationToken)
                )
                .MapAsync(currentRevenueType =>
                {
                    return new Response<RevenueTypeResponse>(null);
                });
        }

        private static Result<EditRevenueTypeRequest> ValidateRequest(
            EditRevenueTypeRequest request
        )
        {
            return request.Id != request.RequestId
                ? Result<EditRevenueTypeRequest>.Failure(
                    RevenueTypeErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditRevenueTypeRequest>.Success(request);
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

        private async Task<Result<RevenueType>> EditAndSaveRevenueTypeAsync(
            RevenueType currentRevenueType,
            EditRevenueTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var editRevenueType = new RevenueType(
                request.Id,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentRevenueType.CreatedAt.GetValueOrDefault(),
                currentRevenueType.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editRevenueType.AddEvent(new RevenueTypeEditedEvent(editRevenueType.Id));

            await ExecuteTransactionAsync(
                () => _revenuetypeDbContext.RevenueTypes.Update(editRevenueType),
                editRevenueType.GetEvents(),
                cancellationToken
            );

            return Result<RevenueType>.Success(editRevenueType);
        }
    }
}
