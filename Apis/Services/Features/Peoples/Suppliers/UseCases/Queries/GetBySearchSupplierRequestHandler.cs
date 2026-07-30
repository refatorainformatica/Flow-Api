using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Suppliers.Exceptions;
using Services.Features.Peoples.Suppliers.Models;
using Services.Features.Peoples.Suppliers.Repositories;
using Services.Features.Peoples.Suppliers.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.Suppliers.UseCases.Queries
{
    public class GetBySearchSupplierRequestHandler(
        IMapper mapper,
        IMediator mediator,
        SupplierDbContext supplierDbContext
    )
        : CommandHandler(supplierDbContext, mediator),
            IRequestHandler<
                GetBySearchSupplierRequest,
                Result<Response<IEnumerable<SupplierResponse>>>
            >
    {
        private readonly SupplierDbContext _supplierDbContext = supplierDbContext;

        public async Task<Result<Response<IEnumerable<SupplierResponse>>>> Handle(
            GetBySearchSupplierRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetBySearchSupplierAsync(request)
                .BindAsync(suppliers => Task.FromResult(GenerateResponse(suppliers)));
        }

        private async Task<Result<Pagination<Supplier>>> GetBySearchSupplierAsync(
            GetBySearchSupplierRequest request
        )
        {
            var suppliers = await Task.Run(
                () =>
                    _supplierDbContext
                        .Suppliers.AsNoTracking()
                        .SearchBy(request.Query.SearchText)
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<Supplier>()
            );

            return !suppliers.Rows.Any()
                ? Result<Pagination<Supplier>>.Failure(
                    SupplierErrors.NotFound(request.Query.SearchText)
                )
                : Result<Pagination<Supplier>>.Success(suppliers);
        }

        private Result<Response<IEnumerable<SupplierResponse>>> GenerateResponse(
            Pagination<Supplier> paginationSupplier
        )
        {
            var supplierResponse = mapper.Map<IEnumerable<SupplierResponse>>(
                paginationSupplier.Rows
            );
            var response = new Response<IEnumerable<SupplierResponse>>(
                supplierResponse,
                paginationSupplier.Offset,
                paginationSupplier.Limit,
                paginationSupplier.PageCount,
                paginationSupplier.RowCount
            );
            return Result<Response<IEnumerable<SupplierResponse>>>.Success(response);
        }
    }
}
