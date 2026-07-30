using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Suppliers.Exceptions;
using Services.Features.Peoples.Suppliers.Models;
using Services.Features.Peoples.Suppliers.Repositories;
using Services.Features.Peoples.Suppliers.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Suppliers.UseCases.Queries
{
    public class GetByIdSupplierRequestHandler(
        IMapper mapper,
        IMediator mediator,
        SupplierDbContext supplierDbContext
    )
        : CommandHandler(supplierDbContext, mediator),
            IRequestHandler<GetByIdSupplierRequest, Result<Response<SupplierResponse>>>
    {
        private readonly SupplierDbContext _supplierDbContext = supplierDbContext;

        public async Task<Result<Response<SupplierResponse>>> Handle(
            GetByIdSupplierRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdSupplierAsync(request, cancellationToken)
                .BindAsync(suppliers => Task.FromResult(GenerateResponse(suppliers)));
        }

        private async Task<Result<Supplier>> GetByIdSupplierAsync(
            GetByIdSupplierRequest request,
            CancellationToken cancellationToken
        )
        {
            var supplier = await _supplierDbContext
                .Suppliers.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return supplier is null
                ? Result<Supplier>.Failure(SupplierErrors.NotFound(request.Id))
                : Result<Supplier>.Success(supplier);
        }

        private Result<Response<SupplierResponse>> GenerateResponse(Supplier supplier)
        {
            var supplierResponse = mapper.Map<SupplierResponse>(supplier);
            var response = new Response<SupplierResponse>(supplierResponse);
            return Result<Response<SupplierResponse>>.Success(response);
        }
    }
}
