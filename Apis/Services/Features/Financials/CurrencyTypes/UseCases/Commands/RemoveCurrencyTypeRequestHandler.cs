using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.CurrencyTypes.Exceptions;
using Services.Features.Financials.CurrencyTypes.Models;
using Services.Features.Financials.CurrencyTypes.Models.Events;
using Services.Features.Financials.CurrencyTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.CurrencyTypes.UseCases.Commands
{
    public class RemoveCurrencyTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        CurrencyTypeDbContext currencytypeDbContext
    )
        : CommandHandler(currencytypeDbContext, mediator),
            IRequestHandler<RemoveCurrencyTypeRequest, Result<Response<CurrencyTypeResponse>>>
    {
        private readonly CurrencyTypeDbContext _currencytypeDbContext = currencytypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<CurrencyTypeResponse>>> Handle(
            RemoveCurrencyTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentCurrencyTypeAsync(req.Id, cancellationToken))
                .BindAsync(currentCurrencyType =>
                    RemoveCurrencyTypeAsync(currentCurrencyType, cancellationToken)
                )
                .MapAsync(currentCurrencyType =>
                {
                    return new Response<CurrencyTypeResponse>(null);
                });
        }

        private static Result<RemoveCurrencyTypeRequest> ValidateRequest(
            RemoveCurrencyTypeRequest request
        )
        {
            return request.Id == default
                ? Result<RemoveCurrencyTypeRequest>.Failure(CurrencyTypeErrors.NotFound(request.Id))
                : Result<RemoveCurrencyTypeRequest>.Success(request);
        }

        private async Task<Result<CurrencyType>> GetCurrentCurrencyTypeAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var currencytype = await _currencytypeDbContext
                .CurrencyTypes.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return currencytype is null
                ? Result<CurrencyType>.Failure(CurrencyTypeErrors.NotFound(id))
                : Result<CurrencyType>.Success(currencytype);
        }

        private async Task<Result<CurrencyType>> RemoveCurrencyTypeAsync(
            CurrencyType removeCurrencyType,
            CancellationToken cancellationToken
        )
        {
            removeCurrencyType.DeletedAt = _dateTimeService.UtcNow;
            removeCurrencyType.EditedAt = _dateTimeService.UtcNow;
            removeCurrencyType.EditedBy = _authenticatedUserService.UserId;

            removeCurrencyType.AddEvent(new CurrencyTypeRemovedEvent(removeCurrencyType.Id));

            await ExecuteTransactionAsync(
                () => _currencytypeDbContext.Update(removeCurrencyType),
                removeCurrencyType.GetEvents(),
                cancellationToken
            );

            return Result<CurrencyType>.Success(removeCurrencyType);
        }
    }
}
