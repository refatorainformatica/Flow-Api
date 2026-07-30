using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.Contracts.Exceptions;
using Services.Features.Financials.Contracts.Models;
using Services.Features.Financials.Contracts.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.Contracts.UseCases.Queries
{
    public class GetBySearchContractRequestHandler(
        IMapper mapper,
        IMediator mediator,
        ContractDbContext contractDbContext
    )
        : CommandHandler(contractDbContext, mediator),
            IRequestHandler<
                GetBySearchContractRequest,
                Result<Response<IEnumerable<ContractResponse>>>
            >
    {
        private readonly ContractDbContext _contractDbContext = contractDbContext;

        public async Task<Result<Response<IEnumerable<ContractResponse>>>> Handle(
            GetBySearchContractRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetBySearchContractAsync(request)
                .BindAsync(contracts => Task.FromResult(GenerateResponse(contracts)));
        }

        private async Task<Result<Pagination<Contract>>> GetBySearchContractAsync(
            GetBySearchContractRequest request
        )
        {
            var contracts = await Task.Run(
                () =>
                    _contractDbContext
                        .Contracts.AsNoTracking()
                        .SearchBy(request.Query.SearchText)
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<Contract>()
            );

            return !contracts.Rows.Any()
                ? Result<Pagination<Contract>>.Failure(
                    ContractErrors.NotFound(request.Query.SearchText)
                )
                : Result<Pagination<Contract>>.Success(contracts);
        }

        private Result<Response<IEnumerable<ContractResponse>>> GenerateResponse(
            Pagination<Contract> paginationContract
        )
        {
            var contractResponse = mapper.Map<IEnumerable<ContractResponse>>(
                paginationContract.Rows
            );
            var response = new Response<IEnumerable<ContractResponse>>(
                contractResponse,
                paginationContract.Offset,
                paginationContract.Limit,
                paginationContract.PageCount,
                paginationContract.RowCount
            );
            return Result<Response<IEnumerable<ContractResponse>>>.Success(response);
        }
    }
}
