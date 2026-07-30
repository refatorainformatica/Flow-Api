using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.SkillTypes.Exceptions;
using Services.Features.Peoples.SkillTypes.Models;
using Services.Features.Peoples.SkillTypes.Repositories;
using Services.Features.Peoples.SkillTypes.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.SkillTypes.UseCases.Queries
{
    public class GetBySearchSkillTypeRequestHandler(
        IMapper mapper,
        IMediator mediator,
        SkillTypeDbContext skilltypeDbContext
    )
        : CommandHandler(skilltypeDbContext, mediator),
            IRequestHandler<
                GetBySearchSkillTypeRequest,
                Result<Response<IEnumerable<SkillTypeResponse>>>
            >
    {
        private readonly SkillTypeDbContext _skilltypeDbContext = skilltypeDbContext;

        public async Task<Result<Response<IEnumerable<SkillTypeResponse>>>> Handle(
            GetBySearchSkillTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetBySearchSkillTypeAsync(request)
                .BindAsync(skilltypes => Task.FromResult(GenerateResponse(skilltypes)));
        }

        private async Task<Result<Pagination<SkillType>>> GetBySearchSkillTypeAsync(
            GetBySearchSkillTypeRequest request
        )
        {
            var skilltypes = await Task.Run(
                () =>
                    _skilltypeDbContext
                        .SkillTypes.AsNoTracking()
                        .SearchBy(request.Query.SearchText)
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<SkillType>()
            );

            return !skilltypes.Rows.Any()
                ? Result<Pagination<SkillType>>.Failure(
                    SkillTypeErrors.NotFound(request.Query.SearchText)
                )
                : Result<Pagination<SkillType>>.Success(skilltypes);
        }

        private Result<Response<IEnumerable<SkillTypeResponse>>> GenerateResponse(
            Pagination<SkillType> paginationSkillType
        )
        {
            var skilltypeResponse = mapper.Map<IEnumerable<SkillTypeResponse>>(
                paginationSkillType.Rows
            );
            var response = new Response<IEnumerable<SkillTypeResponse>>(
                skilltypeResponse,
                paginationSkillType.Offset,
                paginationSkillType.Limit,
                paginationSkillType.PageCount,
                paginationSkillType.RowCount
            );
            return Result<Response<IEnumerable<SkillTypeResponse>>>.Success(response);
        }
    }
}
