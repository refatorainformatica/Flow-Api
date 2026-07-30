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
    public class EditJuridicalNatureRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        JuridicalNatureDbContext juridicalnatureDbContext
    )
        : CommandHandler(juridicalnatureDbContext, mediator),
            IRequestHandler<EditJuridicalNatureRequest, Result<Response<JuridicalNatureResponse>>>
    {
        private readonly JuridicalNatureDbContext _juridicalnatureDbContext =
            juridicalnatureDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<JuridicalNatureResponse>>> Handle(
            EditJuridicalNatureRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentJuridicalNatureAsync(req.Id, cancellationToken))
                .BindAsync(currentJuridicalNature =>
                    EditAndSaveJuridicalNatureAsync(
                        currentJuridicalNature,
                        request,
                        cancellationToken
                    )
                )
                .MapAsync(currentJuridicalNature =>
                {
                    return new Response<JuridicalNatureResponse>(null);
                });
        }

        private static Result<EditJuridicalNatureRequest> ValidateRequest(
            EditJuridicalNatureRequest request
        )
        {
            return request.Id != request.RequestId
                ? Result<EditJuridicalNatureRequest>.Failure(
                    JuridicalNatureErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditJuridicalNatureRequest>.Success(request);
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

        private async Task<Result<JuridicalNature>> EditAndSaveJuridicalNatureAsync(
            JuridicalNature currentJuridicalNature,
            EditJuridicalNatureRequest request,
            CancellationToken cancellationToken
        )
        {
            var editJuridicalNature = new JuridicalNature(
                request.Id,
                request.ExternalCode,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentJuridicalNature.CreatedAt.GetValueOrDefault(),
                currentJuridicalNature.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editJuridicalNature.AddEvent(new JuridicalNatureEditedEvent(editJuridicalNature.Id));

            await ExecuteTransactionAsync(
                () => _juridicalnatureDbContext.JuridicalNatures.Update(editJuridicalNature),
                editJuridicalNature.GetEvents(),
                cancellationToken
            );

            return Result<JuridicalNature>.Success(editJuridicalNature);
        }
    }
}
