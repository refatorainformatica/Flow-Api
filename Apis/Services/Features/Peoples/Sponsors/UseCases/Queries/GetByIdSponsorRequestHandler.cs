using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Sponsors.Exceptions;
using Services.Features.Peoples.Sponsors.Models;
using Services.Features.Peoples.Sponsors.Repositories;
using Services.Features.Peoples.Sponsors.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Sponsors.UseCases.Queries
{
    public class GetByIdSponsorRequestHandler(
        IMapper mapper,
        IMediator mediator,
        SponsorDbContext sponsorDbContext
    )
        : CommandHandler(sponsorDbContext, mediator),
            IRequestHandler<GetByIdSponsorRequest, Result<Response<SponsorResponse>>>
    {
        private readonly SponsorDbContext _sponsorDbContext = sponsorDbContext;

        public async Task<Result<Response<SponsorResponse>>> Handle(
            GetByIdSponsorRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdSponsorAsync(request, cancellationToken)
                .BindAsync(sponsors => Task.FromResult(GenerateResponse(sponsors)));
        }

        private async Task<Result<Sponsor>> GetByIdSponsorAsync(
            GetByIdSponsorRequest request,
            CancellationToken cancellationToken
        )
        {
            var sponsor = await _sponsorDbContext
                .Sponsors.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return sponsor is null
                ? Result<Sponsor>.Failure(SponsorErrors.NotFound(request.Id))
                : Result<Sponsor>.Success(sponsor);
        }

        private Result<Response<SponsorResponse>> GenerateResponse(Sponsor sponsor)
        {
            var sponsorResponse = mapper.Map<SponsorResponse>(sponsor);
            var response = new Response<SponsorResponse>(sponsorResponse);
            return Result<Response<SponsorResponse>>.Success(response);
        }
    }
}
