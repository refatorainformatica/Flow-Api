using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.JuridicalNatures.Exceptions;
using Services.Features.Peoples.JuridicalNatures.Models;
using Services.Features.Peoples.JuridicalNatures.Models.Events;
using Services.Features.Peoples.JuridicalNatures.Repositories;
using Services.Features.Peoples.JuridicalNatures.UseCases.Commands;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.JuridicalNatures.UseCases.Commands
{
    public class RemoveJuridicalNatureRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        JuridicalNatureDbContext juridicalnatureDbContext
    )
        : CommandHandler(juridicalnatureDbContext, mediator),
            IRequestHandler<RemoveJuridicalNatureRequest, Result<Response<JuridicalNatureResponse>>>
    {
        private readonly JuridicalNatureDbContext _juridicalnatureDbContext =
            juridicalnatureDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<JuridicalNatureResponse>>> Handle(
            RemoveJuridicalNatureRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentJuridicalNatureAsync(req.Id, cancellationToken))
                .BindAsync(currentJuridicalNature =>
                    RemoveJuridicalNatureAsync(currentJuridicalNature, cancellationToken)
                )
                .MapAsync(currentJuridicalNature =>
                {
                    return new Response<JuridicalNatureResponse>(null);
                });
        }

        private static Result<RemoveJuridicalNatureRequest> ValidateRequest(
            RemoveJuridicalNatureRequest request
        )
        {
            return request.Id == default
                ? Result<RemoveJuridicalNatureRequest>.Failure(
                    JuridicalNatureErrors.NotFound(request.Id)
                )
                : Result<RemoveJuridicalNatureRequest>.Success(request);
        }

        private async Task<Result<JuridicalNature>> GetCurrentJuridicalNatureAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var juridicalnature = await _juridicalnatureDbContext
                .JuridicalNatures.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return juridicalnature is null
                ? Result<JuridicalNature>.Failure(JuridicalNatureErrors.NotFound(id))
                : Result<JuridicalNature>.Success(juridicalnature);
        }

        private async Task<Result<JuridicalNature>> RemoveJuridicalNatureAsync(
            JuridicalNature removeJuridicalNature,
            CancellationToken cancellationToken
        )
        {
            removeJuridicalNature.DeletedAt = _dateTimeService.UtcNow;
            removeJuridicalNature.EditedAt = _dateTimeService.UtcNow;
            removeJuridicalNature.EditedBy = _authenticatedUserService.UserId;

            removeJuridicalNature.AddEvent(
                new JuridicalNatureRemovedEvent(removeJuridicalNature.Id)
            );

            await ExecuteTransactionAsync(
                () => _juridicalnatureDbContext.Update(removeJuridicalNature),
                removeJuridicalNature.GetEvents(),
                cancellationToken
            );

            return Result<JuridicalNature>.Success(removeJuridicalNature);
        }
    }
}
