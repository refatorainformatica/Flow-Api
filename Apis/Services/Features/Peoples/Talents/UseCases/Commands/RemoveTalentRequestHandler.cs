using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Talents.Exceptions;
using Services.Features.Peoples.Talents.Models;
using Services.Features.Peoples.Talents.Models.Events;
using Services.Features.Peoples.Talents.Repositories;
using Services.Features.Peoples.Talents.UseCases.Commands;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.Talents.UseCases.Commands
{
    public class RemoveTalentRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        TalentDbContext talentDbContext
    )
        : CommandHandler(talentDbContext, mediator),
            IRequestHandler<RemoveTalentRequest, Result<Response<TalentResponse>>>
    {
        private readonly TalentDbContext _talentDbContext = talentDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<TalentResponse>>> Handle(
            RemoveTalentRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentTalentAsync(req.Id, cancellationToken))
                .BindAsync(currentTalent => RemoveTalentAsync(currentTalent, cancellationToken))
                .MapAsync(currentTalent =>
                {
                    return new Response<TalentResponse>(null);
                });
        }

        private static Result<RemoveTalentRequest> ValidateRequest(RemoveTalentRequest request)
        {
            return request.Id == default
                ? Result<RemoveTalentRequest>.Failure(TalentErrors.NotFound(request.Id))
                : Result<RemoveTalentRequest>.Success(request);
        }

        private async Task<Result<Talent>> GetCurrentTalentAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var talent = await _talentDbContext
                .Talents.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return talent is null
                ? Result<Talent>.Failure(TalentErrors.NotFound(id))
                : Result<Talent>.Success(talent);
        }

        private async Task<Result<Talent>> RemoveTalentAsync(
            Talent removeTalent,
            CancellationToken cancellationToken
        )
        {
            removeTalent.DeletedAt = _dateTimeService.UtcNow;
            removeTalent.EditedAt = _dateTimeService.UtcNow;
            removeTalent.EditedBy = _authenticatedUserService.UserId;

            removeTalent.AddEvent(new TalentRemovedEvent(removeTalent.Id));

            await ExecuteTransactionAsync(
                () => _talentDbContext.Update(removeTalent),
                removeTalent.GetEvents(),
                cancellationToken
            );

            return Result<Talent>.Success(removeTalent);
        }
    }
}
