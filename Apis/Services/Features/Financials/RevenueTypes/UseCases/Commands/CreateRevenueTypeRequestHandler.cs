using AutoMapper;
using MediatR;
using Services.Features.Financials.RevenueTypes.Models;
using Services.Features.Financials.RevenueTypes.Models.Events;
using Services.Features.Financials.RevenueTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.RevenueTypes.UseCases.Commands
{
    public class CreateRevenueTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        RevenueTypeDbContext revenuetypeDbContext
    )
        : CommandHandler(revenuetypeDbContext, mediator),
            IRequestHandler<CreateRevenueTypeRequest, Result<Response<RevenueTypeResponse>>>
    {
        private readonly RevenueTypeDbContext _revenuetypeDbContext = revenuetypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<RevenueTypeResponse>>> Handle(
            CreateRevenueTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveRevenueTypeAsync(request, cancellationToken)
                .BindAsync(revenuetype => Task.FromResult(GenerateResponse(revenuetype)));
        }

        private async Task<Result<RevenueType>> SaveRevenueTypeAsync(
            CreateRevenueTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var newRevenueType = new RevenueType(
                0,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newRevenueType.AddEvent(new RevenueTypeCreatedEvent(newRevenueType.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _revenuetypeDbContext.RevenueTypes.AddAsync(
                        newRevenueType,
                        cancellationToken: cancellationToken
                    );
                },
                newRevenueType.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<RevenueType>.Success(newRevenueType);
        }

        private Result<Response<RevenueTypeResponse>> GenerateResponse(RevenueType revenuetype)
        {
            var revenuetypeResponse = mapper.Map<RevenueTypeResponse>(revenuetype);
            var response = new Response<RevenueTypeResponse>(revenuetypeResponse);

            return Result<Response<RevenueTypeResponse>>.Success(response);
        }
    }
}
