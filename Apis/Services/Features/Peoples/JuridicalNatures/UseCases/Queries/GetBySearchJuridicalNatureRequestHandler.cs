using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.JuridicalNatures.Exceptions;
using Services.Features.Peoples.JuridicalNatures.Models;
using Services.Features.Peoples.JuridicalNatures.Repositories;
using Services.Features.Peoples.JuridicalNatures.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.JuridicalNatures.UseCases.Queries
{
    public class GetBySearchJuridicalNatureRequestHandler(
        IMapper mapper,
        IMediator mediator,
        JuridicalNatureDbContext juridicalnatureDbContext
    )
        : CommandHandler(juridicalnatureDbContext, mediator),
            IRequestHandler<
                GetBySearchJuridicalNatureRequest,
                Result<Response<IEnumerable<JuridicalNatureResponse>>>
            >
    {
        private readonly JuridicalNatureDbContext _juridicalnatureDbContext =
            juridicalnatureDbContext;

        public async Task<Result<Response<IEnumerable<JuridicalNatureResponse>>>> Handle(
            GetBySearchJuridicalNatureRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetBySearchJuridicalNatureAsync(request)
                .BindAsync(juridicalnatures => Task.FromResult(GenerateResponse(juridicalnatures)));
        }

        private async Task<Result<Pagination<JuridicalNature>>> GetBySearchJuridicalNatureAsync(
            GetBySearchJuridicalNatureRequest request
        )
        {
            var juridicalnatures = await Task.Run(
                () =>
                    _juridicalnatureDbContext
                        .JuridicalNatures.AsNoTracking()
                        .SearchBy(request.Query.SearchText)
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<JuridicalNature>()
            );

            return !juridicalnatures.Rows.Any()
                ? Result<Pagination<JuridicalNature>>.Failure(
                    JuridicalNatureErrors.NotFound(request.Query.SearchText)
                )
                : Result<Pagination<JuridicalNature>>.Success(juridicalnatures);
        }

        private Result<Response<IEnumerable<JuridicalNatureResponse>>> GenerateResponse(
            Pagination<JuridicalNature> paginationJuridicalNature
        )
        {
            var juridicalnatureResponse = mapper.Map<IEnumerable<JuridicalNatureResponse>>(
                paginationJuridicalNature.Rows
            );
            var response = new Response<IEnumerable<JuridicalNatureResponse>>(
                juridicalnatureResponse,
                paginationJuridicalNature.Offset,
                paginationJuridicalNature.Limit,
                paginationJuridicalNature.PageCount,
                paginationJuridicalNature.RowCount
            );
            return Result<Response<IEnumerable<JuridicalNatureResponse>>>.Success(response);
        }
    }
}
