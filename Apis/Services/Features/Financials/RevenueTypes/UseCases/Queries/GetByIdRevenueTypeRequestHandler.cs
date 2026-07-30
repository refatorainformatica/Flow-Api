using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.RevenueTypes.Exceptions;
using Services.Features.Financials.RevenueTypes.Models;
using Services.Features.Financials.RevenueTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.RevenueTypes.UseCases.Queries
{
    public class GetByIdRevenueTypeRequestHandler(
        IMapper mapper,
        IMediator mediator,
        RevenueTypeDbContext revenuetypeDbContext
    )
        : CommandHandler(revenuetypeDbContext, mediator),
            IRequestHandler<GetByIdRevenueTypeRequest, Result<Response<RevenueTypeResponse>>>
    {
        private readonly RevenueTypeDbContext _revenuetypeDbContext = revenuetypeDbContext;

        public async Task<Result<Response<RevenueTypeResponse>>> Handle(
            GetByIdRevenueTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdRevenueTypeAsync(request, cancellationToken)
                .BindAsync(revenuetypes => Task.FromResult(GenerateResponse(revenuetypes)));
        }

        private async Task<Result<RevenueType>> GetByIdRevenueTypeAsync(
            GetByIdRevenueTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var revenuetype = await _revenuetypeDbContext
                .RevenueTypes.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return revenuetype is null
                ? Result<RevenueType>.Failure(RevenueTypeErrors.NotFound(request.Id))
                : Result<RevenueType>.Success(revenuetype);
        }

        private Result<Response<RevenueTypeResponse>> GenerateResponse(RevenueType revenuetype)
        {
            var revenuetypeResponse = mapper.Map<RevenueTypeResponse>(revenuetype);
            var response = new Response<RevenueTypeResponse>(revenuetypeResponse);
            return Result<Response<RevenueTypeResponse>>.Success(response);
        }
    }
}
