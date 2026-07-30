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
    public class GetTalentRequestHandler(
        IMapper mapper,
        IMediator mediator,
        TalentDbContext talentDbContext
    )
        : CommandHandler(talentDbContext, mediator),
            IRequestHandler<GetTalentRequest, Result<Response<IEnumerable<TalentResponse>>>>
    {
        private readonly TalentDbContext _talentDbContext = talentDbContext;

        public async Task<Result<Response<IEnumerable<TalentResponse>>>> Handle(
            GetTalentRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetTalentAsync(request)
                .BindAsync(talents => Task.FromResult(GenerateResponse(talents)));
        }

        private async Task<Result<Pagination<Talent>>> GetTalentAsync(GetTalentRequest request)
        {
            var talents = await Task.Run(
                () =>
                    _talentDbContext
                        .Talents.AsNoTracking()
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<Talent>()
            );

            return !talents.Rows.Any()
                ? Result<Pagination<Talent>>.Failure(TalentErrors.IsEmpty())
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
