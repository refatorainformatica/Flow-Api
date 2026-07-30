using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.ContractTypes.Exceptions;
using Services.Features.Financials.ContractTypes.Models;
using Services.Features.Financials.ContractTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.ContractTypes.UseCases.Queries
{
    public class GetContractTypeRequestHandler(
        IMapper mapper,
        IMediator mediator,
        ContractTypeDbContext contracttypeDbContext
    )
        : CommandHandler(contracttypeDbContext, mediator),
            IRequestHandler<
                GetContractTypeRequest,
                Result<Response<IEnumerable<ContractTypeResponse>>>
            >
    {
        private readonly ContractTypeDbContext _contracttypeDbContext = contracttypeDbContext;

        public async Task<Result<Response<IEnumerable<ContractTypeResponse>>>> Handle(
            GetContractTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetContractTypeAsync(request)
                .BindAsync(contracttypes => Task.FromResult(GenerateResponse(contracttypes)));
        }

        private async Task<Result<Pagination<ContractType>>> GetContractTypeAsync(
            GetContractTypeRequest request
        )
        {
            var contracttypes = await Task.Run(
                () =>
                    _contracttypeDbContext
                        .ContractTypes.AsNoTracking()
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<ContractType>()
            );

            return !contracttypes.Rows.Any()
                ? Result<Pagination<ContractType>>.Failure(ContractTypeErrors.IsEmpty())
                : Result<Pagination<ContractType>>.Success(contracttypes);
        }

        private Result<Response<IEnumerable<ContractTypeResponse>>> GenerateResponse(
            Pagination<ContractType> paginationContractType
        )
        {
            var contracttypeResponse = mapper.Map<IEnumerable<ContractTypeResponse>>(
                paginationContractType.Rows
            );
            var response = new Response<IEnumerable<ContractTypeResponse>>(
                contracttypeResponse,
                paginationContractType.Offset,
                paginationContractType.Limit,
                paginationContractType.PageCount,
                paginationContractType.RowCount
            );
            return Result<Response<IEnumerable<ContractTypeResponse>>>.Success(response);
        }
    }
}
