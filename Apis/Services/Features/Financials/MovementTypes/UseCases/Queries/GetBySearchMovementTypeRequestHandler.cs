using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.MovementTypes.Exceptions;
using Services.Features.Financials.MovementTypes.Models;
using Services.Features.Financials.MovementTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.MovementTypes.UseCases.Queries
{
    public class GetBySearchMovementTypeRequestHandler(
        IMapper mapper,
        IMediator mediator,
        MovementTypeDbContext movementtypeDbContext
    )
        : CommandHandler(movementtypeDbContext, mediator),
            IRequestHandler<
                GetBySearchMovementTypeRequest,
                Result<Response<IEnumerable<MovementTypeResponse>>>
            >
    {
        private readonly MovementTypeDbContext _movementtypeDbContext = movementtypeDbContext;

        public async Task<Result<Response<IEnumerable<MovementTypeResponse>>>> Handle(
            GetBySearchMovementTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetBySearchMovementTypeAsync(request)
                .BindAsync(movementtypes => Task.FromResult(GenerateResponse(movementtypes)));
        }

        private async Task<Result<Pagination<MovementType>>> GetBySearchMovementTypeAsync(
            GetBySearchMovementTypeRequest request
        )
        {
            var movementtypes = await Task.Run(
                () =>
                    _movementtypeDbContext
                        .MovementTypes.AsNoTracking()
                        .SearchBy(request.Query.SearchText)
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<MovementType>()
            );

            return !movementtypes.Rows.Any()
                ? Result<Pagination<MovementType>>.Failure(
                    MovementTypeErrors.NotFound(request.Query.SearchText)
                )
                : Result<Pagination<MovementType>>.Success(movementtypes);
        }

        private Result<Response<IEnumerable<MovementTypeResponse>>> GenerateResponse(
            Pagination<MovementType> paginationMovementType
        )
        {
            var movementtypeResponse = mapper.Map<IEnumerable<MovementTypeResponse>>(
                paginationMovementType.Rows
            );
            var response = new Response<IEnumerable<MovementTypeResponse>>(
                movementtypeResponse,
                paginationMovementType.Offset,
                paginationMovementType.Limit,
                paginationMovementType.PageCount,
                paginationMovementType.RowCount
            );
            return Result<Response<IEnumerable<MovementTypeResponse>>>.Success(response);
        }
    }
}
