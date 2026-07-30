using AutoMapper;
using MediatR;
using Services.Features.Financials.Banks.Models;
using Services.Features.Financials.Banks.Models.Events;
using Services.Features.Financials.Banks.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.Banks.UseCases.Commands
{
    public class CreateBankRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        BankDbContext bankDbContext
    )
        : CommandHandler(bankDbContext, mediator),
            IRequestHandler<CreateBankRequest, Result<Response<BankResponse>>>
    {
        private readonly BankDbContext _bankDbContext = bankDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<BankResponse>>> Handle(
            CreateBankRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveBankAsync(request, cancellationToken)
                .BindAsync(bank => Task.FromResult(GenerateResponse(bank)));
        }

        private async Task<Result<Bank>> SaveBankAsync(
            CreateBankRequest request,
            CancellationToken cancellationToken
        )
        {
            var newBank = new Bank(
                0,
                request.ExternalCode,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newBank.AddEvent(new BankCreatedEvent(newBank.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _bankDbContext.Banks.AddAsync(
                        newBank,
                        cancellationToken: cancellationToken
                    );
                },
                newBank.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<Bank>.Success(newBank);
        }

        private Result<Response<BankResponse>> GenerateResponse(Bank bank)
        {
            var bankResponse = mapper.Map<BankResponse>(bank);
            var response = new Response<BankResponse>(bankResponse);

            return Result<Response<BankResponse>>.Success(response);
        }
    }
}
