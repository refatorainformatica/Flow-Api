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
    public class EditSupplierRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        SupplierDbContext supplierDbContext
    )
        : CommandHandler(supplierDbContext, mediator),
            IRequestHandler<EditSupplierRequest, Result<Response<SupplierResponse>>>
    {
        private readonly SupplierDbContext _supplierDbContext = supplierDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SupplierResponse>>> Handle(
            EditSupplierRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentSupplierAsync(req.Id, cancellationToken))
                .BindAsync(currentSupplier =>
                    EditAndSaveSupplierAsync(currentSupplier, request, cancellationToken)
                )
                .MapAsync(currentSupplier =>
                {
                    return new Response<SupplierResponse>(null);
                });
        }

        private static Result<EditSupplierRequest> ValidateRequest(EditSupplierRequest request)
        {
            return request.Id != request.RequestId
                ? Result<EditSupplierRequest>.Failure(
                    SupplierErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditSupplierRequest>.Success(request);
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

        private async Task<Result<Supplier>> EditAndSaveSupplierAsync(
            Supplier currentSupplier,
            EditSupplierRequest request,
            CancellationToken cancellationToken
        )
        {
            var editSupplier = new Supplier(
                request.Id,
                request.CompanyName,
                request.CompanyBusinessName,
                request.JuridicalNatureId,
                request.OpeningDate,
                request.ClosingDate,
                request.ActivityBranchId,
                request.PaymentingCurrencyTypeId,
                request.AddressLine1,
                request.AddressLine2,
                request.Email,
                request.PhoneNumber,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                request.TalentId,
                currentSupplier.CreatedAt.GetValueOrDefault(),
                currentSupplier.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            )
            {
                Documents = request
                    .Documents.Select(document => new SupplierDocument()
                    {
                        Id =
                            currentSupplier
                                .Documents.FirstOrDefault(x =>
                                    x.DocumentTypeId == document.DocumentTypeId
                                )
                                ?.Id ?? 0,
                        SupplierId = document.SupplierId,
                        DocumentTypeId = document.DocumentTypeId,
                        EnrollmentCode = document.EnrollmentCode,
                        EnrollmentDate = document.EnrollmentDate,
                        File = document.File,
                        Picture =
                            document.Picture
                            ?? Shared.Infrastructure.Resources.Images.DocumentBase64Image,
                        CreatedAt =
                            currentSupplier
                                .Documents.FirstOrDefault(x =>
                                    x.DocumentTypeId == document.DocumentTypeId
                                )
                                ?.CreatedAt ?? _dateTimeService.UtcNow,
                        CreatedBy =
                            currentSupplier
                                .Documents.FirstOrDefault(x =>
                                    x.DocumentTypeId == document.DocumentTypeId
                                )
                                ?.CreatedBy ?? _authenticatedUserService.UserId,
                        EditedAt = _dateTimeService.UtcNow,
                        EditedBy = _authenticatedUserService.UserId,
                        DeletedAt = document.DeletedAt,
                    })
                    .ToList(),
            };

            editSupplier.AddEvent(new SupplierEditedEvent(editSupplier.Id));

            await ExecuteTransactionAsync(
                () => _supplierDbContext.Suppliers.Update(editSupplier),
                editSupplier.GetEvents(),
                cancellationToken
            );

            return Result<Supplier>.Success(editSupplier);
        }
    }
}
