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
    public class EditBankRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        BankDbContext bankDbContext
    )
        : CommandHandler(bankDbContext, mediator),
            IRequestHandler<EditBankRequest, Result<Response<BankResponse>>>
    {
        private readonly BankDbContext _bankDbContext = bankDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<BankResponse>>> Handle(
            EditBankRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentBankAsync(req.Id, cancellationToken))
                .BindAsync(currentBank =>
                    EditAndSaveBankAsync(currentBank, request, cancellationToken)
                )
                .MapAsync(currentBank =>
                {
                    return new Response<BankResponse>(null);
                });
        }

        private static Result<EditBankRequest> ValidateRequest(EditBankRequest request)
        {
            return request.Id != request.RequestId
                ? Result<EditBankRequest>.Failure(BankErrors.PreConditionFailed(request.RequestId))
                : Result<EditBankRequest>.Success(request);
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

        private async Task<Result<Bank>> EditAndSaveBankAsync(
            Bank currentBank,
            EditBankRequest request,
            CancellationToken cancellationToken
        )
        {
            var editBank = new Bank(
                request.Id,
                request.ExternalCode,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentBank.CreatedAt.GetValueOrDefault(),
                currentBank.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editBank.AddEvent(new BankEditedEvent(editBank.Id));

            await ExecuteTransactionAsync(
                () => _bankDbContext.Banks.Update(editBank),
                editBank.GetEvents(),
                cancellationToken
            );

            return Result<Bank>.Success(editBank);
        }
    }
}
