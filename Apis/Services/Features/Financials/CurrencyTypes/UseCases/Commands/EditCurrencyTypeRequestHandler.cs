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
    public class EditCurrencyTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        CurrencyTypeDbContext currencytypeDbContext
    )
        : CommandHandler(currencytypeDbContext, mediator),
            IRequestHandler<EditCurrencyTypeRequest, Result<Response<CurrencyTypeResponse>>>
    {
        private readonly CurrencyTypeDbContext _currencytypeDbContext = currencytypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<CurrencyTypeResponse>>> Handle(
            EditCurrencyTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentCurrencyTypeAsync(req.Id, cancellationToken))
                .BindAsync(currentCurrencyType =>
                    EditAndSaveCurrencyTypeAsync(currentCurrencyType, request, cancellationToken)
                )
                .MapAsync(currentCurrencyType =>
                {
                    return new Response<CurrencyTypeResponse>(null);
                });
        }

        private static Result<EditCurrencyTypeRequest> ValidateRequest(
            EditCurrencyTypeRequest request
        )
        {
            return request.Id != request.RequestId
                ? Result<EditCurrencyTypeRequest>.Failure(
                    CurrencyTypeErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditCurrencyTypeRequest>.Success(request);
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

        private async Task<Result<CurrencyType>> EditAndSaveCurrencyTypeAsync(
            CurrencyType currentCurrencyType,
            EditCurrencyTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var editCurrencyType = new CurrencyType(
                request.Id,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentCurrencyType.CreatedAt.GetValueOrDefault(),
                currentCurrencyType.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editCurrencyType.AddEvent(new CurrencyTypeEditedEvent(editCurrencyType.Id));

            await ExecuteTransactionAsync(
                () => _currencytypeDbContext.CurrencyTypes.Update(editCurrencyType),
                editCurrencyType.GetEvents(),
                cancellationToken
            );

            return Result<CurrencyType>.Success(editCurrencyType);
        }
    }
}
