using AutoMapper;
using MediatR;
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
    public class CreateSupplierRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        SupplierDbContext supplierDbContext
    )
        : CommandHandler(supplierDbContext, mediator),
            IRequestHandler<CreateSupplierRequest, Result<Response<SupplierResponse>>>
    {
        private readonly SupplierDbContext _supplierDbContext = supplierDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SupplierResponse>>> Handle(
            CreateSupplierRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveSupplierAsync(request, cancellationToken)
                .BindAsync(supplier => Task.FromResult(GenerateResponse(supplier)));
        }

        private async Task<Result<Supplier>> SaveSupplierAsync(
            CreateSupplierRequest request,
            CancellationToken cancellationToken
        )
        {
            var newSupplier = new Supplier(
                0,
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
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            )
            {
                Documents = request
                    .Documents.Select(document => new SupplierDocument()
                    {
                        SupplierId = document.SupplierId,
                        DocumentTypeId = document.DocumentTypeId,
                        EnrollmentCode = document.EnrollmentCode,
                        EnrollmentDate = document.EnrollmentDate,
                        File = document.File,
                        Picture =
                            document.Picture
                            ?? Shared.Infrastructure.Resources.Images.DocumentBase64Image,
                        CreatedAt = _dateTimeService.UtcNow,
                        CreatedBy = _authenticatedUserService.UserId,
                        EditedAt = _dateTimeService.UtcNow,
                        EditedBy = _authenticatedUserService.UserId,
                    })
                    .ToList(),
            };

            newSupplier.AddEvent(new SupplierCreatedEvent(newSupplier.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _supplierDbContext.Suppliers.AddAsync(
                        newSupplier,
                        cancellationToken: cancellationToken
                    );
                },
                newSupplier.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<Supplier>.Success(newSupplier);
        }

        private Result<Response<SupplierResponse>> GenerateResponse(Supplier supplier)
        {
            var supplierResponse = mapper.Map<SupplierResponse>(supplier);
            var response = new Response<SupplierResponse>(supplierResponse);

            return Result<Response<SupplierResponse>>.Success(response);
        }
    }
}
