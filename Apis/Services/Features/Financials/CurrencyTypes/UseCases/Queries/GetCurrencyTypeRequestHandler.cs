using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.CurrencyTypes.Exceptions;
using Services.Features.Financials.CurrencyTypes.Models;
using Services.Features.Financials.CurrencyTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.CurrencyTypes.UseCases.Queries
{
    public class GetCurrencyTypeRequestHandler(
        IMapper mapper,
        IMediator mediator,
        CurrencyTypeDbContext currencytypeDbContext
    )
        : CommandHandler(currencytypeDbContext, mediator),
            IRequestHandler<
                GetCurrencyTypeRequest,
                Result<Response<IEnumerable<CurrencyTypeResponse>>>
            >
    {
        private readonly CurrencyTypeDbContext _currencytypeDbContext = currencytypeDbContext;

        public async Task<Result<Response<IEnumerable<CurrencyTypeResponse>>>> Handle(
            GetCurrencyTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetCurrencyTypeAsync(request)
                .BindAsync(currencytypes => Task.FromResult(GenerateResponse(currencytypes)));
        }

        private async Task<Result<Pagination<CurrencyType>>> GetCurrencyTypeAsync(
            GetCurrencyTypeRequest request
        )
        {
            var currencytypes = await Task.Run(
                () =>
                    _currencytypeDbContext
                        .CurrencyTypes.AsNoTracking()
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<CurrencyType>()
            );

            return !currencytypes.Rows.Any()
                ? Result<Pagination<CurrencyType>>.Failure(CurrencyTypeErrors.IsEmpty())
                : Result<Pagination<CurrencyType>>.Success(currencytypes);
        }

        private Result<Response<IEnumerable<CurrencyTypeResponse>>> GenerateResponse(
            Pagination<CurrencyType> paginationCurrencyType
        )
        {
            var currencytypeResponse = mapper.Map<IEnumerable<CurrencyTypeResponse>>(
                paginationCurrencyType.Rows
            );
            var response = new Response<IEnumerable<CurrencyTypeResponse>>(
                currencytypeResponse,
                paginationCurrencyType.Offset,
                paginationCurrencyType.Limit,
                paginationCurrencyType.PageCount,
                paginationCurrencyType.RowCount
            );
            return Result<Response<IEnumerable<CurrencyTypeResponse>>>.Success(response);
        }
    }
}
