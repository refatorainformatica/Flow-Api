using AutoMapper;
using MediatR;
using Services.Features.Financials.CurrencyTypes.Models;
using Services.Features.Financials.CurrencyTypes.Models.Events;
using Services.Features.Financials.CurrencyTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.CurrencyTypes.UseCases.Commands
{
    public class CreateCurrencyTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        CurrencyTypeDbContext currencytypeDbContext
    )
        : CommandHandler(currencytypeDbContext, mediator),
            IRequestHandler<CreateCurrencyTypeRequest, Result<Response<CurrencyTypeResponse>>>
    {
        private readonly CurrencyTypeDbContext _currencytypeDbContext = currencytypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<CurrencyTypeResponse>>> Handle(
            CreateCurrencyTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveCurrencyTypeAsync(request, cancellationToken)
                .BindAsync(currencytype => Task.FromResult(GenerateResponse(currencytype)));
        }

        private async Task<Result<CurrencyType>> SaveCurrencyTypeAsync(
            CreateCurrencyTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var newCurrencyType = new CurrencyType(
                0,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newCurrencyType.AddEvent(new CurrencyTypeCreatedEvent(newCurrencyType.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _currencytypeDbContext.CurrencyTypes.AddAsync(
                        newCurrencyType,
                        cancellationToken: cancellationToken
                    );
                },
                newCurrencyType.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<CurrencyType>.Success(newCurrencyType);
        }

        private Result<Response<CurrencyTypeResponse>> GenerateResponse(CurrencyType currencytype)
        {
            var currencytypeResponse = mapper.Map<CurrencyTypeResponse>(currencytype);
            var response = new Response<CurrencyTypeResponse>(currencytypeResponse);

            return Result<Response<CurrencyTypeResponse>>.Success(response);
        }
    }
}
