using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.ContractStates.Exceptions;
using Services.Features.Financials.ContractStates.Models;
using Services.Features.Financials.ContractStates.Models.Events;
using Services.Features.Financials.ContractStates.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.ContractStates.UseCases.Commands
{
    public class RemoveContractStateRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        ContractStateDbContext contractstateDbContext
    )
        : CommandHandler(contractstateDbContext, mediator),
            IRequestHandler<RemoveContractStateRequest, Result<Response<ContractStateResponse>>>
    {
        private readonly ContractStateDbContext _contractstateDbContext = contractstateDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<ContractStateResponse>>> Handle(
            RemoveContractStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentContractStateAsync(req.Id, cancellationToken))
                .BindAsync(currentContractState =>
                    RemoveContractStateAsync(currentContractState, cancellationToken)
                )
                .MapAsync(currentContractState =>
                {
                    return new Response<ContractStateResponse>(null);
                });
        }

        private static Result<RemoveContractStateRequest> ValidateRequest(
            RemoveContractStateRequest request
        )
        {
            return request.Id == default
                ? Result<RemoveContractStateRequest>.Failure(
                    ContractStateErrors.NotFound(request.Id)
                )
                : Result<RemoveContractStateRequest>.Success(request);
        }

        private async Task<Result<ContractState>> GetCurrentContractStateAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var contractstate = await _contractstateDbContext
                .ContractStates.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return contractstate is null
                ? Result<ContractState>.Failure(ContractStateErrors.NotFound(id))
                : Result<ContractState>.Success(contractstate);
        }

        private async Task<Result<ContractState>> RemoveContractStateAsync(
            ContractState removeContractState,
            CancellationToken cancellationToken
        )
        {
            removeContractState.DeletedAt = _dateTimeService.UtcNow;
            removeContractState.EditedAt = _dateTimeService.UtcNow;
            removeContractState.EditedBy = _authenticatedUserService.UserId;

            removeContractState.AddEvent(new ContractStateRemovedEvent(removeContractState.Id));

            await ExecuteTransactionAsync(
                () => _contractstateDbContext.Update(removeContractState),
                removeContractState.GetEvents(),
                cancellationToken
            );

            return Result<ContractState>.Success(removeContractState);
        }
    }
}
