using AutoMapper;
using MediatR;
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
    public class CreateSponsorRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        SponsorDbContext sponsorDbContext
    )
        : CommandHandler(sponsorDbContext, mediator),
            IRequestHandler<CreateSponsorRequest, Result<Response<SponsorResponse>>>
    {
        private readonly SponsorDbContext _sponsorDbContext = sponsorDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SponsorResponse>>> Handle(
            CreateSponsorRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveSponsorAsync(request, cancellationToken)
                .BindAsync(sponsor => Task.FromResult(GenerateResponse(sponsor)));
        }

        private async Task<Result<Sponsor>> SaveSponsorAsync(
            CreateSponsorRequest request,
            CancellationToken cancellationToken
        )
        {
            var newSponsor = new Sponsor(
                0,
                request.Name,
                request.AddressLine1,
                request.AddressLine2,
                request.Email,
                request.PhoneNumber,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            )
            {
                Documents = request
                    .Documents.Select(document => new SponsorDocument()
                    {
                        SponsorId = document.SponsorId,
                        DocumentTypeId = document.DocumentTypeId,
                        EnrollmentCode = document.EnrollmentCode,
                        EnrollmentDate = document.EnrollmentDate,
                        File = document.File,
                        Picture =
                            document.Picture
                            ?? Shared.Infrastructure.Resources.Images.DocumentBase64Image,
                        CreatedAt = _dateTimeService.UtcNow,
                        CreatedBy = _authenticatedUserService.UserId,
                        EditedAt = _dateTimeService.UtcNow,
                        EditedBy = _authenticatedUserService.UserId,
                    })
                    .ToList(),
            };

            newSponsor.AddEvent(new SponsorCreatedEvent(newSponsor.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _sponsorDbContext.Sponsors.AddAsync(
                        newSponsor,
                        cancellationToken: cancellationToken
                    );
                },
                newSponsor.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<Sponsor>.Success(newSponsor);
        }

        private Result<Response<SponsorResponse>> GenerateResponse(Sponsor sponsor)
        {
            var sponsorResponse = mapper.Map<SponsorResponse>(sponsor);
            var response = new Response<SponsorResponse>(sponsorResponse);

            return Result<Response<SponsorResponse>>.Success(response);
        }
    }
}
