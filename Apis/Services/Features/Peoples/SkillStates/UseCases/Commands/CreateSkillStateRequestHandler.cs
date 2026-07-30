using AutoMapper;
using MediatR;
using Services.Features.Peoples.SkillStates.Models;
using Services.Features.Peoples.SkillStates.Models.Events;
using Services.Features.Peoples.SkillStates.Repositories;
using Services.Features.Peoples.SkillStates.UseCases.Commands;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.SkillStates.UseCases.Commands
{
    public class CreateSkillStateRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        SkillStateDbContext skillstateDbContext
    )
        : CommandHandler(skillstateDbContext, mediator),
            IRequestHandler<CreateSkillStateRequest, Result<Response<SkillStateResponse>>>
    {
        private readonly SkillStateDbContext _skillstateDbContext = skillstateDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SkillStateResponse>>> Handle(
            CreateSkillStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveSkillStateAsync(request, cancellationToken)
                .BindAsync(skillstate => Task.FromResult(GenerateResponse(skillstate)));
        }

        private async Task<Result<SkillState>> SaveSkillStateAsync(
            CreateSkillStateRequest request,
            CancellationToken cancellationToken
        )
        {
            var newSkillState = new SkillState(
                0,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newSkillState.AddEvent(new SkillStateCreatedEvent(newSkillState.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _skillstateDbContext.SkillStates.AddAsync(
                        newSkillState,
                        cancellationToken: cancellationToken
                    );
                },
                newSkillState.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<SkillState>.Success(newSkillState);
        }

        private Result<Response<SkillStateResponse>> GenerateResponse(SkillState skillstate)
        {
            var skillstateResponse = mapper.Map<SkillStateResponse>(skillstate);
            var response = new Response<SkillStateResponse>(skillstateResponse);

            return Result<Response<SkillStateResponse>>.Success(response);
        }
    }
}
