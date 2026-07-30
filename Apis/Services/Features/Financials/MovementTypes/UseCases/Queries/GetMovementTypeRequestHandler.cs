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
    public class GetMovementTypeRequestHandler(
        IMapper mapper,
        IMediator mediator,
        MovementTypeDbContext movementtypeDbContext
    )
        : CommandHandler(movementtypeDbContext, mediator),
            IRequestHandler<
                GetMovementTypeRequest,
                Result<Response<IEnumerable<MovementTypeResponse>>>
            >
    {
        private readonly MovementTypeDbContext _movementtypeDbContext = movementtypeDbContext;

        public async Task<Result<Response<IEnumerable<MovementTypeResponse>>>> Handle(
            GetMovementTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetMovementTypeAsync(request)
                .BindAsync(movementtypes => Task.FromResult(GenerateResponse(movementtypes)));
        }

        private async Task<Result<Pagination<MovementType>>> GetMovementTypeAsync(
            GetMovementTypeRequest request
        )
        {
            var movementtypes = await Task.Run(
                () =>
                    _movementtypeDbContext
                        .MovementTypes.AsNoTracking()
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<MovementType>()
            );

            return !movementtypes.Rows.Any()
                ? Result<Pagination<MovementType>>.Failure(MovementTypeErrors.IsEmpty())
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
