using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.EducationLevels.Exceptions;
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
    public class RemoveEducationLevelRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        EducationLevelDbContext educationlevelDbContext
    )
        : CommandHandler(educationlevelDbContext, mediator),
            IRequestHandler<RemoveEducationLevelRequest, Result<Response<EducationLevelResponse>>>
    {
        private readonly EducationLevelDbContext _educationlevelDbContext = educationlevelDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<EducationLevelResponse>>> Handle(
            RemoveEducationLevelRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentEducationLevelAsync(req.Id, cancellationToken))
                .BindAsync(currentEducationLevel =>
                    RemoveEducationLevelAsync(currentEducationLevel, cancellationToken)
                )
                .MapAsync(currentEducationLevel =>
                {
                    return new Response<EducationLevelResponse>(null);
                });
        }

        private static Result<RemoveEducationLevelRequest> ValidateRequest(
            RemoveEducationLevelRequest request
        )
        {
            return request.Id == default
                ? Result<RemoveEducationLevelRequest>.Failure(
                    EducationLevelErrors.NotFound(request.Id)
                )
                : Result<RemoveEducationLevelRequest>.Success(request);
        }

        private async Task<Result<EducationLevel>> GetCurrentEducationLevelAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var educationlevel = await _educationlevelDbContext
                .EducationLevels.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return educationlevel is null
                ? Result<EducationLevel>.Failure(EducationLevelErrors.NotFound(id))
                : Result<EducationLevel>.Success(educationlevel);
        }

        private async Task<Result<EducationLevel>> RemoveEducationLevelAsync(
            EducationLevel removeEducationLevel,
            CancellationToken cancellationToken
        )
        {
            removeEducationLevel.DeletedAt = _dateTimeService.UtcNow;
            removeEducationLevel.EditedAt = _dateTimeService.UtcNow;
            removeEducationLevel.EditedBy = _authenticatedUserService.UserId;

            removeEducationLevel.AddEvent(new EducationLevelRemovedEvent(removeEducationLevel.Id));

            await ExecuteTransactionAsync(
                () => _educationlevelDbContext.Update(removeEducationLevel),
                removeEducationLevel.GetEvents(),
                cancellationToken
            );

            return Result<EducationLevel>.Success(removeEducationLevel);
        }
    }
}
