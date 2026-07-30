using AutoMapper;
using MediatR;
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
    public class CreateSellerRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        SellerDbContext sellerDbContext
    )
        : CommandHandler(sellerDbContext, mediator),
            IRequestHandler<CreateSellerRequest, Result<Response<SellerResponse>>>
    {
        private readonly SellerDbContext _sellerDbContext = sellerDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SellerResponse>>> Handle(
            CreateSellerRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveSellerAsync(request, cancellationToken)
                .BindAsync(seller => Task.FromResult(GenerateResponse(seller)));
        }

        private async Task<Result<Seller>> SaveSellerAsync(
            CreateSellerRequest request,
            CancellationToken cancellationToken
        )
        {
            var newSeller = new Seller(
                0,
                request.Name,
                request.AddressLine1,
                request.AddressLine2,
                request.Email,
                request.PhoneNumber,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            )
            {
                Documents = request
                    .Documents.Select(document => new SellerDocument()
                    {
                        SellerId = document.SellerId,
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

            newSeller.AddEvent(new SellerCreatedEvent(newSeller.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _sellerDbContext.Sellers.AddAsync(
                        newSeller,
                        cancellationToken: cancellationToken
                    );
                },
                newSeller.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<Seller>.Success(newSeller);
        }

        private Result<Response<SellerResponse>> GenerateResponse(Seller seller)
        {
            var sellerResponse = mapper.Map<SellerResponse>(seller);
            var response = new Response<SellerResponse>(sellerResponse);

            return Result<Response<SellerResponse>>.Success(response);
        }
    }
}
