using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.EducationLevels.Exceptions;
using Services.Features.Peoples.EducationLevels.Models;
using Services.Features.Peoples.EducationLevels.Repositories;
using Services.Features.Peoples.EducationLevels.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.EducationLevels.UseCases.Queries
{
    public class GetEducationLevelRequestHandler(
        IMapper mapper,
        IMediator mediator,
        EducationLevelDbContext educationlevelDbContext
    )
        : CommandHandler(educationlevelDbContext, mediator),
            IRequestHandler<
                GetEducationLevelRequest,
                Result<Response<IEnumerable<EducationLevelResponse>>>
            >
    {
        private readonly EducationLevelDbContext _educationlevelDbContext = educationlevelDbContext;

        public async Task<Result<Response<IEnumerable<EducationLevelResponse>>>> Handle(
            GetEducationLevelRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetEducationLevelAsync(request)
                .BindAsync(educationlevels => Task.FromResult(GenerateResponse(educationlevels)));
        }

        private async Task<Result<Pagination<EducationLevel>>> GetEducationLevelAsync(
            GetEducationLevelRequest request
        )
        {
            var educationlevels = await Task.Run(
                () =>
                    _educationlevelDbContext
                        .EducationLevels.AsNoTracking()
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<EducationLevel>()
            );

            return !educationlevels.Rows.Any()
                ? Result<Pagination<EducationLevel>>.Failure(EducationLevelErrors.IsEmpty())
                : Result<Pagination<EducationLevel>>.Success(educationlevels);
        }

        private Result<Response<IEnumerable<EducationLevelResponse>>> GenerateResponse(
            Pagination<EducationLevel> paginationEducationLevel
        )
        {
            var educationlevelResponse = mapper.Map<IEnumerable<EducationLevelResponse>>(
                paginationEducationLevel.Rows
            );
            var response = new Response<IEnumerable<EducationLevelResponse>>(
                educationlevelResponse,
                paginationEducationLevel.Offset,
                paginationEducationLevel.Limit,
                paginationEducationLevel.PageCount,
                paginationEducationLevel.RowCount
            );
            return Result<Response<IEnumerable<EducationLevelResponse>>>.Success(response);
        }
    }
}
