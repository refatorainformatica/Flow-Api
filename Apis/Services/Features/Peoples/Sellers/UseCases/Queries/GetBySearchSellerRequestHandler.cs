using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Sellers.Exceptions;
using Services.Features.Peoples.Sellers.Models;
using Services.Features.Peoples.Sellers.Repositories;
using Services.Features.Peoples.Sellers.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.Sellers.UseCases.Queries
{
    public class GetBySearchSellerRequestHandler(
        IMapper mapper,
        IMediator mediator,
        SellerDbContext sellerDbContext
    )
        : CommandHandler(sellerDbContext, mediator),
            IRequestHandler<GetBySearchSellerRequest, Result<Response<IEnumerable<SellerResponse>>>>
    {
        private readonly SellerDbContext _sellerDbContext = sellerDbContext;

        public async Task<Result<Response<IEnumerable<SellerResponse>>>> Handle(
            GetBySearchSellerRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetBySearchSellerAsync(request)
                .BindAsync(sellers => Task.FromResult(GenerateResponse(sellers)));
        }

        private async Task<Result<Pagination<Seller>>> GetBySearchSellerAsync(
            GetBySearchSellerRequest request
        )
        {
            var sellers = await Task.Run(
                () =>
                    _sellerDbContext
                        .Sellers.AsNoTracking()
                        .SearchBy(request.Query.SearchText)
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<Seller>()
            );

            return !sellers.Rows.Any()
                ? Result<Pagination<Seller>>.Failure(
                    SellerErrors.NotFound(request.Query.SearchText)
                )
                : Result<Pagination<Seller>>.Success(sellers);
        }

        private Result<Response<IEnumerable<SellerResponse>>> GenerateResponse(
            Pagination<Seller> paginationSeller
        )
        {
            var sellerResponse = mapper.Map<IEnumerable<SellerResponse>>(paginationSeller.Rows);
            var response = new Response<IEnumerable<SellerResponse>>(
                sellerResponse,
                paginationSeller.Offset,
                paginationSeller.Limit,
                paginationSeller.PageCount,
                paginationSeller.RowCount
            );
            return Result<Response<IEnumerable<SellerResponse>>>.Success(response);
        }
    }
}
