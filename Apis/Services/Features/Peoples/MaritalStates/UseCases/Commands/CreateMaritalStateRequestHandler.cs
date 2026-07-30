using AutoMapper;
using MediatR;
using Services.Features.Peoples.MaritalStates.Models;
using Services.Features.Peoples.MaritalStates.Models.Events;
using Services.Features.Peoples.MaritalStates.Repositories;
using Services.Features.Peoples.MaritalStates.UseCases.Commands;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.MaritalStates.UseCases.Commands
{
    public class CreateMaritalStateRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        MaritalStateDbContext maritalstateDbContext
    )
        : CommandHandler(maritalstateDbContext, mediator),
            IRequestHandler<CreateMaritalStateRequest, Result<Response<MaritalStateResponse>>>
    {
        private readonly MaritalStateDbContext _maritalstateDbContext = maritalstateDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<MaritalStateResponse>>> Handle(
            CreateMaritalStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveMaritalStateAsync(request, cancellationToken)
                .BindAsync(maritalstate => Task.FromResult(GenerateResponse(maritalstate)));
        }

        private async Task<Result<MaritalState>> SaveMaritalStateAsync(
            CreateMaritalStateRequest request,
            CancellationToken cancellationToken
        )
        {
            var newMaritalState = new MaritalState(
                0,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newMaritalState.AddEvent(new MaritalStateCreatedEvent(newMaritalState.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _maritalstateDbContext.MaritalStates.AddAsync(
                        newMaritalState,
                        cancellationToken: cancellationToken
                    );
                },
                newMaritalState.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<MaritalState>.Success(newMaritalState);
        }

        private Result<Response<MaritalStateResponse>> GenerateResponse(MaritalState maritalstate)
        {
            var maritalstateResponse = mapper.Map<MaritalStateResponse>(maritalstate);
            var response = new Response<MaritalStateResponse>(maritalstateResponse);

            return Result<Response<MaritalStateResponse>>.Success(response);
        }
    }
}
