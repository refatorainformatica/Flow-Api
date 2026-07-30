using AutoMapper;
using MediatR;
using Services.Features.Financials.ContractStates.Models;
using Services.Features.Financials.ContractStates.Models.Events;
using Services.Features.Financials.ContractStates.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.ContractStates.UseCases.Commands
{
    public class CreateContractStateRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        ContractStateDbContext contractstateDbContext
    )
        : CommandHandler(contractstateDbContext, mediator),
            IRequestHandler<CreateContractStateRequest, Result<Response<ContractStateResponse>>>
    {
        private readonly ContractStateDbContext _contractstateDbContext = contractstateDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<ContractStateResponse>>> Handle(
            CreateContractStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveContractStateAsync(request, cancellationToken)
                .BindAsync(contractstate => Task.FromResult(GenerateResponse(contractstate)));
        }

        private async Task<Result<ContractState>> SaveContractStateAsync(
            CreateContractStateRequest request,
            CancellationToken cancellationToken
        )
        {
            var newContractState = new ContractState(
                0,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newContractState.AddEvent(new ContractStateCreatedEvent(newContractState.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _contractstateDbContext.ContractStates.AddAsync(
                        newContractState,
                        cancellationToken: cancellationToken
                    );
                },
                newContractState.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<ContractState>.Success(newContractState);
        }

        private Result<Response<ContractStateResponse>> GenerateResponse(
            ContractState contractstate
        )
        {
            var contractstateResponse = mapper.Map<ContractStateResponse>(contractstate);
            var response = new Response<ContractStateResponse>(contractstateResponse);

            return Result<Response<ContractStateResponse>>.Success(response);
        }
    }
}
