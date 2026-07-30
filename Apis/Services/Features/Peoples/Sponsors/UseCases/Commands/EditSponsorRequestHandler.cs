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
    public class EditSponsorRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        SponsorDbContext sponsorDbContext
    )
        : CommandHandler(sponsorDbContext, mediator),
            IRequestHandler<EditSponsorRequest, Result<Response<SponsorResponse>>>
    {
        private readonly SponsorDbContext _sponsorDbContext = sponsorDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SponsorResponse>>> Handle(
            EditSponsorRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentSponsorAsync(req.Id, cancellationToken))
                .BindAsync(currentSponsor =>
                    EditAndSaveSponsorAsync(currentSponsor, request, cancellationToken)
                )
                .MapAsync(currentSponsor =>
                {
                    return new Response<SponsorResponse>(null);
                });
        }

        private static Result<EditSponsorRequest> ValidateRequest(EditSponsorRequest request)
        {
            return request.Id != request.RequestId
                ? Result<EditSponsorRequest>.Failure(
                    SponsorErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditSponsorRequest>.Success(request);
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

        private async Task<Result<Sponsor>> EditAndSaveSponsorAsync(
            Sponsor currentSponsor,
            EditSponsorRequest request,
            CancellationToken cancellationToken
        )
        {
            var editSponsor = new Sponsor(
                request.Id,
                request.Name,
                request.AddressLine1,
                request.AddressLine2,
                request.Email,
                request.PhoneNumber,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentSponsor.CreatedAt.GetValueOrDefault(),
                currentSponsor.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            )
            {
                Documents = request
                    .Documents.Select(document => new SponsorDocument()
                    {
                        Id =
                            currentSponsor
                                .Documents.FirstOrDefault(x =>
                                    x.DocumentTypeId == document.DocumentTypeId
                                )
                                ?.Id ?? 0,
                        SponsorId = document.SponsorId,
                        DocumentTypeId = document.DocumentTypeId,
                        EnrollmentCode = document.EnrollmentCode,
                        EnrollmentDate = document.EnrollmentDate,
                        File = document.File,
                        Picture =
                            document.Picture
                            ?? Shared.Infrastructure.Resources.Images.DocumentBase64Image,
                        CreatedAt =
                            currentSponsor
                                .Documents.FirstOrDefault(x =>
                                    x.DocumentTypeId == document.DocumentTypeId
                                )
                                ?.CreatedAt ?? _dateTimeService.UtcNow,
                        CreatedBy =
                            currentSponsor
                                .Documents.FirstOrDefault(x =>
                                    x.DocumentTypeId == document.DocumentTypeId
                                )
                                ?.CreatedBy ?? _authenticatedUserService.UserId,
                        EditedAt = _dateTimeService.UtcNow,
                        EditedBy = _authenticatedUserService.UserId,
                        DeletedAt = document.DeletedAt,
                    })
                    .ToList(),
            };

            editSponsor.AddEvent(new SponsorEditedEvent(editSponsor.Id));

            await ExecuteTransactionAsync(
                () => _sponsorDbContext.Sponsors.Update(editSponsor),
                editSponsor.GetEvents(),
                cancellationToken
            );

            return Result<Sponsor>.Success(editSponsor);
        }
    }
}
