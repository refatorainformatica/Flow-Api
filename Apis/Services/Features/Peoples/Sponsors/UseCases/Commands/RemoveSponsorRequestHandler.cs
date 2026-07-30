using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Sponsors.Exceptions;
using Services.Features.Peoples.Sponsors.Models;
using Services.Features.Peoples.Sponsors.Models.Events;
using Services.Features.Peoples.Sponsors.Repositories;
using Services.Features.Peoples.Sponsors.UseCases.Commands;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.Sponsors.UseCases.Commands
{
    public class RemoveSponsorRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        SponsorDbContext sponsorDbContext
    )
        : CommandHandler(sponsorDbContext, mediator),
            IRequestHandler<RemoveSponsorRequest, Result<Response<SponsorResponse>>>
    {
        private readonly SponsorDbContext _sponsorDbContext = sponsorDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SponsorResponse>>> Handle(
            RemoveSponsorRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentSponsorAsync(req.Id, cancellationToken))
                .BindAsync(currentSponsor => RemoveSponsorAsync(currentSponsor, cancellationToken))
                .MapAsync(currentSponsor =>
                {
                    return new Response<SponsorResponse>(null);
                });
        }

        private static Result<RemoveSponsorRequest> ValidateRequest(RemoveSponsorRequest request)
        {
            return request.Id == default
                ? Result<RemoveSponsorRequest>.Failure(SponsorErrors.NotFound(request.Id))
                : Result<RemoveSponsorRequest>.Success(request);
        }

        private async Task<Result<Sponsor>> GetCurrentSponsorAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var sponsor = await _sponsorDbContext
                .Sponsors.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return sponsor is null
                ? Result<Sponsor>.Failure(SponsorErrors.NotFound(id))
                : Result<Sponsor>.Success(sponsor);
        }

        private async Task<Result<Sponsor>> RemoveSponsorAsync(
            Sponsor removeSponsor,
            CancellationToken cancellationToken
        )
        {
            removeSponsor.DeletedAt = _dateTimeService.UtcNow;
            removeSponsor.EditedAt = _dateTimeService.UtcNow;
            removeSponsor.EditedBy = _authenticatedUserService.UserId;

            removeSponsor.AddEvent(new SponsorRemovedEvent(removeSponsor.Id));

            await ExecuteTransactionAsync(
                () => _sponsorDbContext.Update(removeSponsor),
                removeSponsor.GetEvents(),
                cancellationToken
            );

            return Result<Sponsor>.Success(removeSponsor);
        }
    }
}
