using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Talents.Exceptions;
using Services.Features.Peoples.Talents.Models;
using Services.Features.Peoples.Talents.Repositories;
using Services.Features.Peoples.Talents.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.Talents.UseCases.Queries
{
    public class GetBySearchTalentRequestHandler(
        IMapper mapper,
        IMediator mediator,
        TalentDbContext talentDbContext
    )
        : CommandHandler(talentDbContext, mediator),
            IRequestHandler<GetBySearchTalentRequest, Result<Response<IEnumerable<TalentResponse>>>>
    {
        private readonly TalentDbContext _talentDbContext = talentDbContext;

        public async Task<Result<Response<IEnumerable<TalentResponse>>>> Handle(
            GetBySearchTalentRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetBySearchTalentAsync(request)
                .BindAsync(talents => Task.FromResult(GenerateResponse(talents)));
        }

        private async Task<Result<Pagination<Talent>>> GetBySearchTalentAsync(
            GetBySearchTalentRequest request
        )
        {
            var talents = await Task.Run(
                () =>
                    _talentDbContext
                        .Talents.AsNoTracking()
                        .SearchBy(request.Query.SearchText)
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<Talent>()
            );

            return !talents.Rows.Any()
                ? Result<Pagination<Talent>>.Failure(
                    TalentErrors.NotFound(request.Query.SearchText)
                )
                : Result<Pagination<Talent>>.Success(talents);
        }

        private Result<Response<IEnumerable<TalentResponse>>> GenerateResponse(
            Pagination<Talent> paginationTalent
        )
        {
            var talentResponse = mapper.Map<IEnumerable<TalentResponse>>(paginationTalent.Rows);
            var response = new Response<IEnumerable<TalentResponse>>(
                talentResponse,
                paginationTalent.Offset,
                paginationTalent.Limit,
                paginationTalent.PageCount,
                paginationTalent.RowCount
            );
            return Result<Response<IEnumerable<TalentResponse>>>.Success(response);
        }
    }
}
