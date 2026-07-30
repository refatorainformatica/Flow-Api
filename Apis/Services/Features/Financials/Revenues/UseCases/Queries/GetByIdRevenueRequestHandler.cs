////using AutoMapper;
////using MediatR;
////using Services.Features.Financials.Revenues.Exceptions;
////using Services.Features.Financials.Revenues.Models;
////using Services.Features.Financials.Revenues.Repositories;
////using Shared.Domain.Abstractions.Bus;
////using Shared.Domain.Abstractions.Primitives;
////
////namespace Services.Features.Financials.Revenues.UseCases.Queries
////{
////    public class GetByIdRevenueRequestHandler(
////        IMapper mapper,
////        IMediator mediator,
////        RevenueDbContext revenueDbContext
////    )
////        : CommandHandler(revenueDbContext, mediator),
////            IRequestHandler<GetByIdRevenueRequest, Result<Response<RevenueResponse>>>
////    {
////        private readonly RevenueDbContext _revenueDbContext = revenueDbContext;
////
////        public async Task<Result<Response<RevenueResponse>>> Handle(
////            GetByIdRevenueRequest request,
////            CancellationToken cancellationToken
////        )
////        {
////            return await GetByIdRevenueAsync(request, cancellationToken)
////                .BindAsync(revenues => Task.FromResult(GenerateResponse(revenues)));
////        }
////
////        private async Task<Result<Revenue>> GetByIdRevenueAsync(
////            GetByIdRevenueRequest request,
////            CancellationToken cancellationToken
////        )
////        {
////            var revenue = await _revenueDbContext.Revenues.FindAsync(
////                [request.Id],
////                cancellationToken
////            );
////
////            return revenue is null
////                ? Result<Revenue>.Failure(RevenueErrors.NotFound(request.Id))
////                : Result<Revenue>.Success(revenue);
////        }
////
////        private Result<Response<RevenueResponse>> GenerateResponse(Revenue revenue)
////        {
////            var revenueResponse = mapper.Map<RevenueResponse>(revenue);
////            var response = new Response<RevenueResponse>(revenueResponse);
////            return Result<Response<RevenueResponse>>.Success(response);
////        }
////    }
////}
//using AutoMapper;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using Services.Features.Financials.ByIdRevenues.Exceptions;
//using Services.Features.Financials.ByIdRevenues.Models;
//using Services.Features.Financials.ByIdRevenues.Repositories;
//using Shared.Domain.Abstractions.Bus;
//using Shared.Domain.Abstractions.Primitives;
//using Shared.Infrastructure.Extensions;
//
//namespace Services.Features.Financials.ByIdRevenues.UseCases.Queries
//{
//    public class GetByIdRevenueRequestHandler(
//        IMapper mapper,
//        IMediator mediator,
//        ByIdRevenueDbContext byidrevenueDbContext
//    )
//        : CommandHandler(byidrevenueDbContext, mediator),
//            IRequestHandler<GetByIdRevenueRequest, Result<Response<IEnumerable<ByIdRevenueResponse>>>>
//    {
//        private readonly ByIdRevenueDbContext _byidrevenueDbContext = byidrevenueDbContext;
//
//        public async Task<Result<Response<IEnumerable<ByIdRevenueResponse>>>> Handle(
//            GetByIdRevenueRequest request,
//            CancellationToken cancellationToken
//        )
//        {
//            return await GetByIdRevenueAsync(request)
//                .BindAsync(byidrevenues => Task.FromResult(GenerateResponse(byidrevenues)));
//        }
//
//        private async Task<Result<Pagination<ByIdRevenue>>> GetByIdRevenueAsync(GetByIdRevenueRequest request)
//        {
//            var byidrevenues = await Task.Run(
//                () =>
//                    _byidrevenueDbContext
//                        .ByIdRevenues.AsNoTracking()
//                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
//                        .CreatePagination(request.Query.Offset, request.Query.Limit)
//                    ?? new Pagination<ByIdRevenue>()
//            );
//
//            return !byidrevenues.Rows.Any()
//                ? Result<Pagination<ByIdRevenue>>.Failure(ByIdRevenueErrors.IsEmpty())
//                : Result<Pagination<ByIdRevenue>>.Success(byidrevenues);
//        }
//
//        private Result<Response<IEnumerable<ByIdRevenueResponse>>> GenerateResponse(
//            Pagination<ByIdRevenue> paginationByIdRevenue
//        )
//        {
//            var byidrevenueResponse = mapper.Map<IEnumerable<ByIdRevenueResponse>>(paginationByIdRevenue.Rows);
//            var response = new Response<IEnumerable<ByIdRevenueResponse>>(
//                byidrevenueResponse,
//                paginationByIdRevenue.Offset,
//                paginationByIdRevenue.Limit,
//                paginationByIdRevenue.PageCount,
//                paginationByIdRevenue.RowCount
//            );
//            return Result<Response<IEnumerable<ByIdRevenueResponse>>>.Success(response);
//        }
//    }
//}
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.Revenues.Exceptions;
using Services.Features.Financials.Revenues.Models;
using Services.Features.Financials.Revenues.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Revenues.UseCases.Queries
{
    public class GetByIdRevenueRequestHandler(
        IMapper mapper,
        IMediator mediator,
        RevenueDbContext revenueDbContext
    )
        : CommandHandler(revenueDbContext, mediator),
            IRequestHandler<GetByIdRevenueRequest, Result<Response<RevenueResponse>>>
    {
        private readonly RevenueDbContext _revenueDbContext = revenueDbContext;

        public async Task<Result<Response<RevenueResponse>>> Handle(
            GetByIdRevenueRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdRevenueAsync(request, cancellationToken)
                .BindAsync(revenues => Task.FromResult(GenerateResponse(revenues)));
        }

        private async Task<Result<Revenue>> GetByIdRevenueAsync(
            GetByIdRevenueRequest request,
            CancellationToken cancellationToken
        )
        {
            var revenue = await _revenueDbContext
                .Revenues.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return revenue is null
                ? Result<Revenue>.Failure(RevenueErrors.NotFound(request.Id))
                : Result<Revenue>.Success(revenue);
        }

        private Result<Response<RevenueResponse>> GenerateResponse(Revenue revenue)
        {
            var revenueResponse = mapper.Map<RevenueResponse>(revenue);
            var response = new Response<RevenueResponse>(revenueResponse);
            return Result<Response<RevenueResponse>>.Success(response);
        }
    }
}
