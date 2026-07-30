using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.ContractStates.Exceptions;
using Services.Features.Financials.ContractStates.Models;
using Services.Features.Financials.ContractStates.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.ContractStates.UseCases.Queries
{
    public class GetContractStateRequestHandler(
        IMapper mapper,
        IMediator mediator,
        ContractStateDbContext contractstateDbContext
    )
        : CommandHandler(contractstateDbContext, mediator),
            IRequestHandler<
                GetContractStateRequest,
                Result<Response<IEnumerable<ContractStateResponse>>>
            >
    {
        private readonly ContractStateDbContext _contractstateDbContext = contractstateDbContext;

        public async Task<Result<Response<IEnumerable<ContractStateResponse>>>> Handle(
            GetContractStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetContractStateAsync(request)
                .BindAsync(contractstates => Task.FromResult(GenerateResponse(contractstates)));
        }

        private async Task<Result<Pagination<ContractState>>> GetContractStateAsync(
            GetContractStateRequest request
        )
        {
            var contractstates = await Task.Run(
                () =>
                    _contractstateDbContext
                        .ContractStates.AsNoTracking()
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<ContractState>()
            );

            return !contractstates.Rows.Any()
                ? Result<Pagination<ContractState>>.Failure(ContractStateErrors.IsEmpty())
                : Result<Pagination<ContractState>>.Success(contractstates);
        }

        private Result<Response<IEnumerable<ContractStateResponse>>> GenerateResponse(
            Pagination<ContractState> paginationContractState
        )
        {
            var contractstateResponse = mapper.Map<IEnumerable<ContractStateResponse>>(
                paginationContractState.Rows
            );
            var response = new Response<IEnumerable<ContractStateResponse>>(
                contractstateResponse,
                paginationContractState.Offset,
                paginationContractState.Limit,
                paginationContractState.PageCount,
                paginationContractState.RowCount
            );
            return Result<Response<IEnumerable<ContractStateResponse>>>.Success(response);
        }
    }
}
