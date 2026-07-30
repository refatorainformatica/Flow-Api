using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.MovementTypes.Exceptions;
using Services.Features.Financials.MovementTypes.Models;
using Services.Features.Financials.MovementTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.MovementTypes.UseCases.Queries
{
    public class GetByIdMovementTypeRequestHandler(
        IMapper mapper,
        IMediator mediator,
        MovementTypeDbContext movementtypeDbContext
    )
        : CommandHandler(movementtypeDbContext, mediator),
            IRequestHandler<GetByIdMovementTypeRequest, Result<Response<MovementTypeResponse>>>
    {
        private readonly MovementTypeDbContext _movementtypeDbContext = movementtypeDbContext;

        public async Task<Result<Response<MovementTypeResponse>>> Handle(
            GetByIdMovementTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdMovementTypeAsync(request, cancellationToken)
                .BindAsync(movementtypes => Task.FromResult(GenerateResponse(movementtypes)));
        }

        private async Task<Result<MovementType>> GetByIdMovementTypeAsync(
            GetByIdMovementTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var movementtype = await _movementtypeDbContext
                .MovementTypes.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return movementtype is null
                ? Result<MovementType>.Failure(MovementTypeErrors.NotFound(request.Id))
                : Result<MovementType>.Success(movementtype);
        }

        private Result<Response<MovementTypeResponse>> GenerateResponse(MovementType movementtype)
        {
            var movementtypeResponse = mapper.Map<MovementTypeResponse>(movementtype);
            var response = new Response<MovementTypeResponse>(movementtypeResponse);
            return Result<Response<MovementTypeResponse>>.Success(response);
        }
    }
}
