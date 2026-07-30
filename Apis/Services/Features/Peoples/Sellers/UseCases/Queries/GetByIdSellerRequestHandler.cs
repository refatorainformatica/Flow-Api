using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Sellers.Exceptions;
using Services.Features.Peoples.Sellers.Models;
using Services.Features.Peoples.Sellers.Repositories;
using Services.Features.Peoples.Sellers.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Sellers.UseCases.Queries
{
    public class GetByIdSellerRequestHandler(
        IMapper mapper,
        IMediator mediator,
        SellerDbContext sellerDbContext
    )
        : CommandHandler(sellerDbContext, mediator),
            IRequestHandler<GetByIdSellerRequest, Result<Response<SellerResponse>>>
    {
        private readonly SellerDbContext _sellerDbContext = sellerDbContext;

        public async Task<Result<Response<SellerResponse>>> Handle(
            GetByIdSellerRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdSellerAsync(request, cancellationToken)
                .BindAsync(sellers => Task.FromResult(GenerateResponse(sellers)));
        }

        private async Task<Result<Seller>> GetByIdSellerAsync(
            GetByIdSellerRequest request,
            CancellationToken cancellationToken
        )
        {
            var seller = await _sellerDbContext
                .Sellers.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return seller is null
                ? Result<Seller>.Failure(SellerErrors.NotFound(request.Id))
                : Result<Seller>.Success(seller);
        }

        private Result<Response<SellerResponse>> GenerateResponse(Seller seller)
        {
            var sellerResponse = mapper.Map<SellerResponse>(seller);
            var response = new Response<SellerResponse>(sellerResponse);
            return Result<Response<SellerResponse>>.Success(response);
        }
    }
}
