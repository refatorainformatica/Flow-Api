using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Sellers.Exceptions;
using Services.Features.Peoples.Sellers.Models;
using Services.Features.Peoples.Sellers.Models.Events;
using Services.Features.Peoples.Sellers.Repositories;
using Services.Features.Peoples.Sellers.UseCases.Commands;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.Sellers.UseCases.Commands
{
    public class EditSellerRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        SellerDbContext sellerDbContext
    )
        : CommandHandler(sellerDbContext, mediator),
            IRequestHandler<EditSellerRequest, Result<Response<SellerResponse>>>
    {
        private readonly SellerDbContext _sellerDbContext = sellerDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SellerResponse>>> Handle(
            EditSellerRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentSellerAsync(req.Id, cancellationToken))
                .BindAsync(currentSeller =>
                    EditAndSaveSellerAsync(currentSeller, request, cancellationToken)
                )
                .MapAsync(currentSeller =>
                {
                    return new Response<SellerResponse>(null);
                });
        }

        private static Result<EditSellerRequest> ValidateRequest(EditSellerRequest request)
        {
            return request.Id != request.RequestId
                ? Result<EditSellerRequest>.Failure(
                    SellerErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditSellerRequest>.Success(request);
        }

        private async Task<Result<Seller>> GetCurrentSellerAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var seller = await _sellerDbContext
                .Sellers.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return seller is null
                ? Result<Seller>.Failure(SellerErrors.NotFound(id))
                : Result<Seller>.Success(seller);
        }

        private async Task<Result<Seller>> EditAndSaveSellerAsync(
            Seller currentSeller,
            EditSellerRequest request,
            CancellationToken cancellationToken
        )
        {
            var editSeller = new Seller(
                request.Id,
                request.Name,
                request.AddressLine1,
                request.AddressLine2,
                request.Email,
                request.PhoneNumber,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentSeller.CreatedAt.GetValueOrDefault(),
                currentSeller.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            )
            {
                Documents = request
                    .Documents.Select(document => new SellerDocument()
                    {
                        Id =
                            currentSeller
                                .Documents.FirstOrDefault(x =>
                                    x.DocumentTypeId == document.DocumentTypeId
                                )
                                ?.Id ?? 0,
                        SellerId = document.SellerId,
                        DocumentTypeId = document.DocumentTypeId,
                        EnrollmentCode = document.EnrollmentCode,
                        EnrollmentDate = document.EnrollmentDate,
                        File = document.File,
                        Picture =
                            document.Picture
                            ?? Shared.Infrastructure.Resources.Images.DocumentBase64Image,
                        CreatedAt =
                            currentSeller
                                .Documents.FirstOrDefault(x =>
                                    x.DocumentTypeId == document.DocumentTypeId
                                )
                                ?.CreatedAt ?? _dateTimeService.UtcNow,
                        CreatedBy =
                            currentSeller
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

            editSeller.AddEvent(new SellerEditedEvent(editSeller.Id));

            await ExecuteTransactionAsync(
                () => _sellerDbContext.Sellers.Update(editSeller),
                editSeller.GetEvents(),
                cancellationToken
            );

            return Result<Seller>.Success(editSeller);
        }
    }
}
