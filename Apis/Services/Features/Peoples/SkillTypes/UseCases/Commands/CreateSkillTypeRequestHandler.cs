using AutoMapper;
using MediatR;
using Services.Features.Peoples.SkillTypes.Models;
using Services.Features.Peoples.SkillTypes.Models.Events;
using Services.Features.Peoples.SkillTypes.Repositories;
using Services.Features.Peoples.SkillTypes.UseCases.Commands;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.SkillTypes.UseCases.Commands
{
    public class CreateSkillTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        SkillTypeDbContext skilltypeDbContext
    )
        : CommandHandler(skilltypeDbContext, mediator),
            IRequestHandler<CreateSkillTypeRequest, Result<Response<SkillTypeResponse>>>
    {
        private readonly SkillTypeDbContext _skilltypeDbContext = skilltypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SkillTypeResponse>>> Handle(
            CreateSkillTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveSkillTypeAsync(request, cancellationToken)
                .BindAsync(skilltype => Task.FromResult(GenerateResponse(skilltype)));
        }

        private async Task<Result<SkillType>> SaveSkillTypeAsync(
            CreateSkillTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var newSkillType = new SkillType(
                0,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newSkillType.AddEvent(new SkillTypeCreatedEvent(newSkillType.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _skilltypeDbContext.SkillTypes.AddAsync(
                        newSkillType,
                        cancellationToken: cancellationToken
                    );
                },
                newSkillType.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<SkillType>.Success(newSkillType);
        }

        private Result<Response<SkillTypeResponse>> GenerateResponse(SkillType skilltype)
        {
            var skilltypeResponse = mapper.Map<SkillTypeResponse>(skilltype);
            var response = new Response<SkillTypeResponse>(skilltypeResponse);

            return Result<Response<SkillTypeResponse>>.Success(response);
        }
    }
}
