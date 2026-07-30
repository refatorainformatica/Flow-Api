using AutoMapper;
using MediatR;
using Services.Features.Peoples.SkillLevels.Models;
using Services.Features.Peoples.SkillLevels.Models.Events;
using Services.Features.Peoples.SkillLevels.Repositories;
using Services.Features.Peoples.SkillLevels.UseCases.Commands;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.SkillLevels.UseCases.Commands
{
    public class CreateSkillLevelRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        SkillLevelDbContext skilllevelDbContext
    )
        : CommandHandler(skilllevelDbContext, mediator),
            IRequestHandler<CreateSkillLevelRequest, Result<Response<SkillLevelResponse>>>
    {
        private readonly SkillLevelDbContext _skilllevelDbContext = skilllevelDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SkillLevelResponse>>> Handle(
            CreateSkillLevelRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveSkillLevelAsync(request, cancellationToken)
                .BindAsync(skilllevel => Task.FromResult(GenerateResponse(skilllevel)));
        }

        private async Task<Result<SkillLevel>> SaveSkillLevelAsync(
            CreateSkillLevelRequest request,
            CancellationToken cancellationToken
        )
        {
            var newSkillLevel = new SkillLevel(
                0,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newSkillLevel.AddEvent(new SkillLevelCreatedEvent(newSkillLevel.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _skilllevelDbContext.SkillLevels.AddAsync(
                        newSkillLevel,
                        cancellationToken: cancellationToken
                    );
                },
                newSkillLevel.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<SkillLevel>.Success(newSkillLevel);
        }

        private Result<Response<SkillLevelResponse>> GenerateResponse(SkillLevel skilllevel)
        {
            var skilllevelResponse = mapper.Map<SkillLevelResponse>(skilllevel);
            var response = new Response<SkillLevelResponse>(skilllevelResponse);

            return Result<Response<SkillLevelResponse>>.Success(response);
        }
    }
}
