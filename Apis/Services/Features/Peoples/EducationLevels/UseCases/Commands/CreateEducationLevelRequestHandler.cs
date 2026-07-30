using AutoMapper;
using MediatR;
using Services.Features.Peoples.EducationLevels.Models;
using Services.Features.Peoples.EducationLevels.Models.Events;
using Services.Features.Peoples.EducationLevels.Repositories;
using Services.Features.Peoples.EducationLevels.UseCases.Commands;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.EducationLevels.UseCases.Commands
{
    public class CreateEducationLevelRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        EducationLevelDbContext educationlevelDbContext
    )
        : CommandHandler(educationlevelDbContext, mediator),
            IRequestHandler<CreateEducationLevelRequest, Result<Response<EducationLevelResponse>>>
    {
        private readonly EducationLevelDbContext _educationlevelDbContext = educationlevelDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<EducationLevelResponse>>> Handle(
            CreateEducationLevelRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveEducationLevelAsync(request, cancellationToken)
                .BindAsync(educationlevel => Task.FromResult(GenerateResponse(educationlevel)));
        }

        private async Task<Result<EducationLevel>> SaveEducationLevelAsync(
            CreateEducationLevelRequest request,
            CancellationToken cancellationToken
        )
        {
            var newEducationLevel = new EducationLevel(
                0,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newEducationLevel.AddEvent(new EducationLevelCreatedEvent(newEducationLevel.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _educationlevelDbContext.EducationLevels.AddAsync(
                        newEducationLevel,
                        cancellationToken: cancellationToken
                    );
                },
                newEducationLevel.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<EducationLevel>.Success(newEducationLevel);
        }

        private Result<Response<EducationLevelResponse>> GenerateResponse(
            EducationLevel educationlevel
        )
        {
            var educationlevelResponse = mapper.Map<EducationLevelResponse>(educationlevel);
            var response = new Response<EducationLevelResponse>(educationlevelResponse);

            return Result<Response<EducationLevelResponse>>.Success(response);
        }
    }
}
