using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.Contracts.Exceptions;
using Services.Features.Financials.Contracts.Models;
using Services.Features.Financials.Contracts.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Contracts.UseCases.Queries
{
    public class GetByIdContractRequestHandler(
        IMapper mapper,
        IMediator mediator,
        ContractDbContext contractDbContext
    )
        : CommandHandler(contractDbContext, mediator),
            IRequestHandler<GetByIdContractRequest, Result<Response<ContractResponse>>>
    {
        private readonly ContractDbContext _contractDbContext = contractDbContext;

        public async Task<Result<Response<ContractResponse>>> Handle(
            GetByIdContractRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdContractAsync(request, cancellationToken)
                .BindAsync(contracts => Task.FromResult(GenerateResponse(contracts)));
        }

        private async Task<Result<Contract>> GetByIdContractAsync(
            GetByIdContractRequest request,
            CancellationToken cancellationToken
        )
        {
            var contract = await _contractDbContext
                .Contracts.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return contract is null
                ? Result<Contract>.Failure(ContractErrors.NotFound(request.Id))
                : Result<Contract>.Success(contract);
        }

        private Result<Response<ContractResponse>> GenerateResponse(Contract contract)
        {
            var contractResponse = mapper.Map<ContractResponse>(contract);
            var response = new Response<ContractResponse>(contractResponse);
            return Result<Response<ContractResponse>>.Success(response);
        }
    }
}
