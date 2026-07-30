using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.ContractStates.Exceptions;
using Services.Features.Financials.ContractStates.Models;
using Services.Features.Financials.ContractStates.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.ContractStates.UseCases.Queries
{
    public class GetByIdContractStateRequestHandler(
        IMapper mapper,
        IMediator mediator,
        ContractStateDbContext contractstateDbContext
    )
        : CommandHandler(contractstateDbContext, mediator),
            IRequestHandler<GetByIdContractStateRequest, Result<Response<ContractStateResponse>>>
    {
        private readonly ContractStateDbContext _contractstateDbContext = contractstateDbContext;

        public async Task<Result<Response<ContractStateResponse>>> Handle(
            GetByIdContractStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdContractStateAsync(request, cancellationToken)
                .BindAsync(contractstates => Task.FromResult(GenerateResponse(contractstates)));
        }

        private async Task<Result<ContractState>> GetByIdContractStateAsync(
            GetByIdContractStateRequest request,
            CancellationToken cancellationToken
        )
        {
            var contractstate = await _contractstateDbContext
                .ContractStates.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return contractstate is null
                ? Result<ContractState>.Failure(ContractStateErrors.NotFound(request.Id))
                : Result<ContractState>.Success(contractstate);
        }

        private Result<Response<ContractStateResponse>> GenerateResponse(
            ContractState contractstate
        )
        {
            var contractstateResponse = mapper.Map<ContractStateResponse>(contractstate);
            var response = new Response<ContractStateResponse>(contractstateResponse);
            return Result<Response<ContractStateResponse>>.Success(response);
        }
    }
}
