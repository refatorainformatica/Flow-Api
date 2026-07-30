using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.MaritalStates.Exceptions;
using Services.Features.Peoples.MaritalStates.Models;
using Services.Features.Peoples.MaritalStates.Repositories;
using Services.Features.Peoples.MaritalStates.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.MaritalStates.UseCases.Queries
{
    public class GetByIdMaritalStateRequestHandler(
        IMapper mapper,
        IMediator mediator,
        MaritalStateDbContext maritalstateDbContext
    )
        : CommandHandler(maritalstateDbContext, mediator),
            IRequestHandler<GetByIdMaritalStateRequest, Result<Response<MaritalStateResponse>>>
    {
        private readonly MaritalStateDbContext _maritalstateDbContext = maritalstateDbContext;

        public async Task<Result<Response<MaritalStateResponse>>> Handle(
            GetByIdMaritalStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdMaritalStateAsync(request, cancellationToken)
                .BindAsync(maritalstates => Task.FromResult(GenerateResponse(maritalstates)));
        }

        private async Task<Result<MaritalState>> GetByIdMaritalStateAsync(
            GetByIdMaritalStateRequest request,
            CancellationToken cancellationToken
        )
        {
            var maritalstate = await _maritalstateDbContext
                .MaritalStates.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return maritalstate is null
                ? Result<MaritalState>.Failure(MaritalStateErrors.NotFound(request.Id))
                : Result<MaritalState>.Success(maritalstate);
        }

        private Result<Response<MaritalStateResponse>> GenerateResponse(MaritalState maritalstate)
        {
            var maritalstateResponse = mapper.Map<MaritalStateResponse>(maritalstate);
            var response = new Response<MaritalStateResponse>(maritalstateResponse);
            return Result<Response<MaritalStateResponse>>.Success(response);
        }
    }
}
