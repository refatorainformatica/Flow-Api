using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.CurrencyTypes.Exceptions;
using Services.Features.Financials.CurrencyTypes.Models;
using Services.Features.Financials.CurrencyTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.CurrencyTypes.UseCases.Queries
{
    public class GetByIdCurrencyTypeRequestHandler(
        IMapper mapper,
        IMediator mediator,
        CurrencyTypeDbContext currencytypeDbContext
    )
        : CommandHandler(currencytypeDbContext, mediator),
            IRequestHandler<GetByIdCurrencyTypeRequest, Result<Response<CurrencyTypeResponse>>>
    {
        private readonly CurrencyTypeDbContext _currencytypeDbContext = currencytypeDbContext;

        public async Task<Result<Response<CurrencyTypeResponse>>> Handle(
            GetByIdCurrencyTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdCurrencyTypeAsync(request, cancellationToken)
                .BindAsync(currencytypes => Task.FromResult(GenerateResponse(currencytypes)));
        }

        private async Task<Result<CurrencyType>> GetByIdCurrencyTypeAsync(
            GetByIdCurrencyTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var currencytype = await _currencytypeDbContext
                .CurrencyTypes.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return currencytype is null
                ? Result<CurrencyType>.Failure(CurrencyTypeErrors.NotFound(request.Id))
                : Result<CurrencyType>.Success(currencytype);
        }

        private Result<Response<CurrencyTypeResponse>> GenerateResponse(CurrencyType currencytype)
        {
            var currencytypeResponse = mapper.Map<CurrencyTypeResponse>(currencytype);
            var response = new Response<CurrencyTypeResponse>(currencytypeResponse);
            return Result<Response<CurrencyTypeResponse>>.Success(response);
        }
    }
}
