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
    public class EditContractStateRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        ContractStateDbContext contractstateDbContext
    )
        : CommandHandler(contractstateDbContext, mediator),
            IRequestHandler<EditContractStateRequest, Result<Response<ContractStateResponse>>>
    {
        private readonly ContractStateDbContext _contractstateDbContext = contractstateDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<ContractStateResponse>>> Handle(
            EditContractStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentContractStateAsync(req.Id, cancellationToken))
                .BindAsync(currentContractState =>
                    EditAndSaveContractStateAsync(currentContractState, request, cancellationToken)
                )
                .MapAsync(currentContractState =>
                {
                    return new Response<ContractStateResponse>(null);
                });
        }

        private static Result<EditContractStateRequest> ValidateRequest(
            EditContractStateRequest request
        )
        {
            return request.Id != request.RequestId
                ? Result<EditContractStateRequest>.Failure(
                    ContractStateErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditContractStateRequest>.Success(request);
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

        private async Task<Result<ContractState>> EditAndSaveContractStateAsync(
            ContractState currentContractState,
            EditContractStateRequest request,
            CancellationToken cancellationToken
        )
        {
            var editContractState = new ContractState(
                request.Id,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentContractState.CreatedAt.GetValueOrDefault(),
                currentContractState.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editContractState.AddEvent(new ContractStateEditedEvent(editContractState.Id));

            await ExecuteTransactionAsync(
                () => _contractstateDbContext.ContractStates.Update(editContractState),
                editContractState.GetEvents(),
                cancellationToken
            );

            return Result<ContractState>.Success(editContractState);
        }
    }
}
