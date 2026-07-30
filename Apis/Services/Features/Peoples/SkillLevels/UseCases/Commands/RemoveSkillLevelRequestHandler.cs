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
    public class RemoveSkillLevelRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        SkillLevelDbContext skilllevelDbContext
    )
        : CommandHandler(skilllevelDbContext, mediator),
            IRequestHandler<RemoveSkillLevelRequest, Result<Response<SkillLevelResponse>>>
    {
        private readonly SkillLevelDbContext _skilllevelDbContext = skilllevelDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SkillLevelResponse>>> Handle(
            RemoveSkillLevelRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentSkillLevelAsync(req.Id, cancellationToken))
                .BindAsync(currentSkillLevel =>
                    RemoveSkillLevelAsync(currentSkillLevel, cancellationToken)
                )
                .MapAsync(currentSkillLevel =>
                {
                    return new Response<SkillLevelResponse>(null);
                });
        }

        private static Result<RemoveSkillLevelRequest> ValidateRequest(
            RemoveSkillLevelRequest request
        )
        {
            return request.Id == default
                ? Result<RemoveSkillLevelRequest>.Failure(SkillLevelErrors.NotFound(request.Id))
                : Result<RemoveSkillLevelRequest>.Success(request);
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

        private async Task<Result<SkillLevel>> RemoveSkillLevelAsync(
            SkillLevel removeSkillLevel,
            CancellationToken cancellationToken
        )
        {
            removeSkillLevel.DeletedAt = _dateTimeService.UtcNow;
            removeSkillLevel.EditedAt = _dateTimeService.UtcNow;
            removeSkillLevel.EditedBy = _authenticatedUserService.UserId;

            removeSkillLevel.AddEvent(new SkillLevelRemovedEvent(removeSkillLevel.Id));

            await ExecuteTransactionAsync(
                () => _skilllevelDbContext.Update(removeSkillLevel),
                removeSkillLevel.GetEvents(),
                cancellationToken
            );

            return Result<SkillLevel>.Success(removeSkillLevel);
        }
    }
}
