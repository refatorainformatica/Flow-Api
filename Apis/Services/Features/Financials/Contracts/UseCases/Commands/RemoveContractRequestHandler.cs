using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.Contracts.Exceptions;
using Services.Features.Financials.Contracts.Models;
using Services.Features.Financials.Contracts.Models.Events;
using Services.Features.Financials.Contracts.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.Contracts.UseCases.Commands
{
    public class RemoveContractRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        ContractDbContext contractDbContext
    )
        : CommandHandler(contractDbContext, mediator),
            IRequestHandler<RemoveContractRequest, Result<Response<ContractResponse>>>
    {
        private readonly ContractDbContext _contractDbContext = contractDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<ContractResponse>>> Handle(
            RemoveContractRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentContractAsync(req.Id, cancellationToken))
                .BindAsync(currentContract =>
                    RemoveContractAsync(currentContract, cancellationToken)
                )
                .MapAsync(currentContract =>
                {
                    return new Response<ContractResponse>(null);
                });
        }

        private static Result<RemoveContractRequest> ValidateRequest(RemoveContractRequest request)
        {
            return request.Id == default
                ? Result<RemoveContractRequest>.Failure(ContractErrors.NotFound(request.Id))
                : Result<RemoveContractRequest>.Success(request);
        }

        private async Task<Result<Contract>> GetCurrentContractAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var contract = await _contractDbContext
                .Contracts.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return contract is null
                ? Result<Contract>.Failure(ContractErrors.NotFound(id))
                : Result<Contract>.Success(contract);
        }

        private async Task<Result<Contract>> RemoveContractAsync(
            Contract removeContract,
            CancellationToken cancellationToken
        )
        {
            removeContract.DeletedAt = _dateTimeService.UtcNow;
            removeContract.EditedAt = _dateTimeService.UtcNow;
            removeContract.EditedBy = _authenticatedUserService.UserId;

            removeContract.AddEvent(new ContractRemovedEvent(removeContract.Id));

            await ExecuteTransactionAsync(
                () => _contractDbContext.Update(removeContract),
                removeContract.GetEvents(),
                cancellationToken
            );

            return Result<Contract>.Success(removeContract);
        }
    }
}
