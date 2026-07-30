using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.ContractTypes.Exceptions;
using Services.Features.Financials.ContractTypes.Models;
using Services.Features.Financials.ContractTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.ContractTypes.UseCases.Queries
{
    public class GetByIdContractTypeRequestHandler(
        IMapper mapper,
        IMediator mediator,
        ContractTypeDbContext contracttypeDbContext
    )
        : CommandHandler(contracttypeDbContext, mediator),
            IRequestHandler<GetByIdContractTypeRequest, Result<Response<ContractTypeResponse>>>
    {
        private readonly ContractTypeDbContext _contracttypeDbContext = contracttypeDbContext;

        public async Task<Result<Response<ContractTypeResponse>>> Handle(
            GetByIdContractTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdContractTypeAsync(request, cancellationToken)
                .BindAsync(contracttypes => Task.FromResult(GenerateResponse(contracttypes)));
        }

        private async Task<Result<ContractType>> GetByIdContractTypeAsync(
            GetByIdContractTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var contracttype = await _contracttypeDbContext
                .ContractTypes.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return contracttype is null
                ? Result<ContractType>.Failure(ContractTypeErrors.NotFound(request.Id))
                : Result<ContractType>.Success(contracttype);
        }

        private Result<Response<ContractTypeResponse>> GenerateResponse(ContractType contracttype)
        {
            var contracttypeResponse = mapper.Map<ContractTypeResponse>(contracttype);
            var response = new Response<ContractTypeResponse>(contracttypeResponse);
            return Result<Response<ContractTypeResponse>>.Success(response);
        }
    }
}
