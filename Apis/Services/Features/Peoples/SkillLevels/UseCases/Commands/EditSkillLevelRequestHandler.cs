using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.SkillLevels.Exceptions;
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
    public class EditSkillLevelRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        SkillLevelDbContext skilllevelDbContext
    )
        : CommandHandler(skilllevelDbContext, mediator),
            IRequestHandler<EditSkillLevelRequest, Result<Response<SkillLevelResponse>>>
    {
        private readonly SkillLevelDbContext _skilllevelDbContext = skilllevelDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SkillLevelResponse>>> Handle(
            EditSkillLevelRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentSkillLevelAsync(req.Id, cancellationToken))
                .BindAsync(currentSkillLevel =>
                    EditAndSaveSkillLevelAsync(currentSkillLevel, request, cancellationToken)
                )
                .MapAsync(currentSkillLevel =>
                {
                    return new Response<SkillLevelResponse>(null);
                });
        }

        private static Result<EditSkillLevelRequest> ValidateRequest(EditSkillLevelRequest request)
        {
            return request.Id != request.RequestId
                ? Result<EditSkillLevelRequest>.Failure(
                    SkillLevelErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditSkillLevelRequest>.Success(request);
        }

        private async Task<Result<SkillLevel>> GetCurrentSkillLevelAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var skilllevel = await _skilllevelDbContext
                .SkillLevels.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return skilllevel is null
                ? Result<SkillLevel>.Failure(SkillLevelErrors.NotFound(id))
                : Result<SkillLevel>.Success(skilllevel);
        }

        private async Task<Result<SkillLevel>> EditAndSaveSkillLevelAsync(
            SkillLevel currentSkillLevel,
            EditSkillLevelRequest request,
            CancellationToken cancellationToken
        )
        {
            var editSkillLevel = new SkillLevel(
                request.Id,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentSkillLevel.CreatedAt.GetValueOrDefault(),
                currentSkillLevel.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editSkillLevel.AddEvent(new SkillLevelEditedEvent(editSkillLevel.Id));

            await ExecuteTransactionAsync(
                () => _skilllevelDbContext.SkillLevels.Update(editSkillLevel),
                editSkillLevel.GetEvents(),
                cancellationToken
            );

            return Result<SkillLevel>.Success(editSkillLevel);
        }
    }
}
