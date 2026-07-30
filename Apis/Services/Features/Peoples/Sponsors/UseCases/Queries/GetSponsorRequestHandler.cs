using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Sponsors.Exceptions;
using Services.Features.Peoples.Sponsors.Models;
using Services.Features.Peoples.Sponsors.Repositories;
using Services.Features.Peoples.Sponsors.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.Sponsors.UseCases.Queries
{
    public class GetSponsorRequestHandler(
        IMapper mapper,
        IMediator mediator,
        SponsorDbContext sponsorDbContext
    )
        : CommandHandler(sponsorDbContext, mediator),
            IRequestHandler<GetSponsorRequest, Result<Response<IEnumerable<SponsorResponse>>>>
    {
        private readonly SponsorDbContext _sponsorDbContext = sponsorDbContext;

        public async Task<Result<Response<IEnumerable<SponsorResponse>>>> Handle(
            GetSponsorRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetSponsorAsync(request)
                .BindAsync(sponsors => Task.FromResult(GenerateResponse(sponsors)));
        }

        private async Task<Result<Pagination<Sponsor>>> GetSponsorAsync(GetSponsorRequest request)
        {
            var sponsors = await Task.Run(
                () =>
                    _sponsorDbContext
                        .Sponsors.AsNoTracking()
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<Sponsor>()
            );

            return !sponsors.Rows.Any()
                ? Result<Pagination<Sponsor>>.Failure(SponsorErrors.IsEmpty())
                : Result<Pagination<Sponsor>>.Success(sponsors);
        }

        private Result<Response<IEnumerable<SponsorResponse>>> GenerateResponse(
            Pagination<Sponsor> paginationSponsor
        )
        {
            var sponsorResponse = mapper.Map<IEnumerable<SponsorResponse>>(paginationSponsor.Rows);
            var response = new Response<IEnumerable<SponsorResponse>>(
                sponsorResponse,
                paginationSponsor.Offset,
                paginationSponsor.Limit,
                paginationSponsor.PageCount,
                paginationSponsor.RowCount
            );
            return Result<Response<IEnumerable<SponsorResponse>>>.Success(response);
        }
    }
}
