using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Suppliers.Exceptions;
using Services.Features.Peoples.Suppliers.Models;
using Services.Features.Peoples.Suppliers.Models.Events;
using Services.Features.Peoples.Suppliers.Repositories;
using Services.Features.Peoples.Suppliers.UseCases.Commands;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.Suppliers.UseCases.Commands
{
    public class RemoveSupplierRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        SupplierDbContext supplierDbContext
    )
        : CommandHandler(supplierDbContext, mediator),
            IRequestHandler<RemoveSupplierRequest, Result<Response<SupplierResponse>>>
    {
        private readonly SupplierDbContext _supplierDbContext = supplierDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SupplierResponse>>> Handle(
            RemoveSupplierRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentSupplierAsync(req.Id, cancellationToken))
                .BindAsync(currentSupplier =>
                    RemoveSupplierAsync(currentSupplier, cancellationToken)
                )
                .MapAsync(currentSupplier =>
                {
                    return new Response<SupplierResponse>(null);
                });
        }

        private static Result<RemoveSupplierRequest> ValidateRequest(RemoveSupplierRequest request)
        {
            return request.Id == default
                ? Result<RemoveSupplierRequest>.Failure(SupplierErrors.NotFound(request.Id))
                : Result<RemoveSupplierRequest>.Success(request);
        }

        private async Task<Result<Supplier>> GetCurrentSupplierAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var supplier = await _supplierDbContext
                .Suppliers.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return supplier is null
                ? Result<Supplier>.Failure(SupplierErrors.NotFound(id))
                : Result<Supplier>.Success(supplier);
        }

        private async Task<Result<Supplier>> RemoveSupplierAsync(
            Supplier removeSupplier,
            CancellationToken cancellationToken
        )
        {
            removeSupplier.DeletedAt = _dateTimeService.UtcNow;
            removeSupplier.EditedAt = _dateTimeService.UtcNow;
            removeSupplier.EditedBy = _authenticatedUserService.UserId;

            removeSupplier.AddEvent(new SupplierRemovedEvent(removeSupplier.Id));

            await ExecuteTransactionAsync(
                () => _supplierDbContext.Update(removeSupplier),
                removeSupplier.GetEvents(),
                cancellationToken
            );

            return Result<Supplier>.Success(removeSupplier);
        }
    }
}
