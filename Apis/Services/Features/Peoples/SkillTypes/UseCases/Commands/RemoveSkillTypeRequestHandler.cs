using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.SkillTypes.Exceptions;
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
    public class RemoveSkillTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        SkillTypeDbContext skilltypeDbContext
    )
        : CommandHandler(skilltypeDbContext, mediator),
            IRequestHandler<RemoveSkillTypeRequest, Result<Response<SkillTypeResponse>>>
    {
        private readonly SkillTypeDbContext _skilltypeDbContext = skilltypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SkillTypeResponse>>> Handle(
            RemoveSkillTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentSkillTypeAsync(req.Id, cancellationToken))
                .BindAsync(currentSkillType =>
                    RemoveSkillTypeAsync(currentSkillType, cancellationToken)
                )
                .MapAsync(currentSkillType =>
                {
                    return new Response<SkillTypeResponse>(null);
                });
        }

        private static Result<RemoveSkillTypeRequest> ValidateRequest(
            RemoveSkillTypeRequest request
        )
        {
            return request.Id == default
                ? Result<RemoveSkillTypeRequest>.Failure(SkillTypeErrors.NotFound(request.Id))
                : Result<RemoveSkillTypeRequest>.Success(request);
        }

        private async Task<Result<SkillType>> GetCurrentSkillTypeAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var skilltype = await _skilltypeDbContext
                .SkillTypes.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return skilltype is null
                ? Result<SkillType>.Failure(SkillTypeErrors.NotFound(id))
                : Result<SkillType>.Success(skilltype);
        }

        private async Task<Result<SkillType>> RemoveSkillTypeAsync(
            SkillType removeSkillType,
            CancellationToken cancellationToken
        )
        {
            removeSkillType.DeletedAt = _dateTimeService.UtcNow;
            removeSkillType.EditedAt = _dateTimeService.UtcNow;
            removeSkillType.EditedBy = _authenticatedUserService.UserId;

            removeSkillType.AddEvent(new SkillTypeRemovedEvent(removeSkillType.Id));

            await ExecuteTransactionAsync(
                () => _skilltypeDbContext.Update(removeSkillType),
                removeSkillType.GetEvents(),
                cancellationToken
            );

            return Result<SkillType>.Success(removeSkillType);
        }
    }
}
