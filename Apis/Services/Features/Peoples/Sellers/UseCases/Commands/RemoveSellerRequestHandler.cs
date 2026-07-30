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
    public class RemoveSellerRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        SellerDbContext sellerDbContext
    )
        : CommandHandler(sellerDbContext, mediator),
            IRequestHandler<RemoveSellerRequest, Result<Response<SellerResponse>>>
    {
        private readonly SellerDbContext _sellerDbContext = sellerDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SellerResponse>>> Handle(
            RemoveSellerRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentSellerAsync(req.Id, cancellationToken))
                .BindAsync(currentSeller => RemoveSellerAsync(currentSeller, cancellationToken))
                .MapAsync(currentSeller =>
                {
                    return new Response<SellerResponse>(null);
                });
        }

        private static Result<RemoveSellerRequest> ValidateRequest(RemoveSellerRequest request)
        {
            return request.Id == default
                ? Result<RemoveSellerRequest>.Failure(SellerErrors.NotFound(request.Id))
                : Result<RemoveSellerRequest>.Success(request);
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

        private async Task<Result<Seller>> RemoveSellerAsync(
            Seller removeSeller,
            CancellationToken cancellationToken
        )
        {
            removeSeller.DeletedAt = _dateTimeService.UtcNow;
            removeSeller.EditedAt = _dateTimeService.UtcNow;
            removeSeller.EditedBy = _authenticatedUserService.UserId;

            removeSeller.AddEvent(new SellerRemovedEvent(removeSeller.Id));

            await ExecuteTransactionAsync(
                () => _sellerDbContext.Update(removeSeller),
                removeSeller.GetEvents(),
                cancellationToken
            );

            return Result<Seller>.Success(removeSeller);
        }
    }
}
