using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.MaritalStates.Exceptions;
using Services.Features.Peoples.MaritalStates.Models;
using Services.Features.Peoples.MaritalStates.Repositories;
using Services.Features.Peoples.MaritalStates.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.MaritalStates.UseCases.Queries
{
    public class GetBySearchMaritalStateRequestHandler(
        IMapper mapper,
        IMediator mediator,
        MaritalStateDbContext maritalstateDbContext
    )
        : CommandHandler(maritalstateDbContext, mediator),
            IRequestHandler<
                GetBySearchMaritalStateRequest,
                Result<Response<IEnumerable<MaritalStateResponse>>>
            >
    {
        private readonly MaritalStateDbContext _maritalstateDbContext = maritalstateDbContext;

        public async Task<Result<Response<IEnumerable<MaritalStateResponse>>>> Handle(
            GetBySearchMaritalStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetBySearchMaritalStateAsync(request)
                .BindAsync(maritalstates => Task.FromResult(GenerateResponse(maritalstates)));
        }

        private async Task<Result<Pagination<MaritalState>>> GetBySearchMaritalStateAsync(
            GetBySearchMaritalStateRequest request
        )
        {
            var maritalstates = await Task.Run(
                () =>
                    _maritalstateDbContext
                        .MaritalStates.AsNoTracking()
                        .SearchBy(request.Query.SearchText)
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<MaritalState>()
            );

            return !maritalstates.Rows.Any()
                ? Result<Pagination<MaritalState>>.Failure(
                    MaritalStateErrors.NotFound(request.Query.SearchText)
                )
                : Result<Pagination<MaritalState>>.Success(maritalstates);
        }

        private Result<Response<IEnumerable<MaritalStateResponse>>> GenerateResponse(
            Pagination<MaritalState> paginationMaritalState
        )
        {
            var maritalstateResponse = mapper.Map<IEnumerable<MaritalStateResponse>>(
                paginationMaritalState.Rows
            );
            var response = new Response<IEnumerable<MaritalStateResponse>>(
                maritalstateResponse,
                paginationMaritalState.Offset,
                paginationMaritalState.Limit,
                paginationMaritalState.PageCount,
                paginationMaritalState.RowCount
            );
            return Result<Response<IEnumerable<MaritalStateResponse>>>.Success(response);
        }
    }
}
