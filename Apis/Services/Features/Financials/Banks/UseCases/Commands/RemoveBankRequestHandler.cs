using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.Banks.Exceptions;
using Services.Features.Financials.Banks.Models;
using Services.Features.Financials.Banks.Models.Events;
using Services.Features.Financials.Banks.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.Banks.UseCases.Commands
{
    public class RemoveBankRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        BankDbContext bankDbContext
    )
        : CommandHandler(bankDbContext, mediator),
            IRequestHandler<RemoveBankRequest, Result<Response<BankResponse>>>
    {
        private readonly BankDbContext _bankDbContext = bankDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<BankResponse>>> Handle(
            RemoveBankRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentBankAsync(req.Id, cancellationToken))
                .BindAsync(currentBank => RemoveBankAsync(currentBank, cancellationToken))
                .MapAsync(currentBank =>
                {
                    return new Response<BankResponse>(null);
                });
        }

        private static Result<RemoveBankRequest> ValidateRequest(RemoveBankRequest request)
        {
            return request.Id == default
                ? Result<RemoveBankRequest>.Failure(BankErrors.NotFound(request.Id))
                : Result<RemoveBankRequest>.Success(request);
        }

        private async Task<Result<Bank>> GetCurrentBankAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var bank = await _bankDbContext
                .Banks.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return bank is null
                ? Result<Bank>.Failure(BankErrors.NotFound(id))
                : Result<Bank>.Success(bank);
        }

        private async Task<Result<Bank>> RemoveBankAsync(
            Bank removeBank,
            CancellationToken cancellationToken
        )
        {
            removeBank.DeletedAt = _dateTimeService.UtcNow;
            removeBank.EditedAt = _dateTimeService.UtcNow;
            removeBank.EditedBy = _authenticatedUserService.UserId;

            removeBank.AddEvent(new BankRemovedEvent(removeBank.Id));

            await ExecuteTransactionAsync(
                () => _bankDbContext.Update(removeBank),
                removeBank.GetEvents(),
                cancellationToken
            );

            return Result<Bank>.Success(removeBank);
        }
    }
}
