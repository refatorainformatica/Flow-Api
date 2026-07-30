using AutoMapper;
using MediatR;
using Services.Features.Financials.Contracts.Models;
using Services.Features.Financials.Contracts.Models.Events;
using Services.Features.Financials.Contracts.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.Contracts.UseCases.Commands
{
    public class CreateContractRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        ContractDbContext contractDbContext
    )
        : CommandHandler(contractDbContext, mediator),
            IRequestHandler<CreateContractRequest, Result<Response<ContractResponse>>>
    {
        private readonly ContractDbContext _contractDbContext = contractDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<ContractResponse>>> Handle(
            CreateContractRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveContractAsync(request, cancellationToken)
                .BindAsync(contract => Task.FromResult(GenerateResponse(contract)));
        }

        private async Task<Result<Contract>> SaveContractAsync(
            CreateContractRequest request,
            CancellationToken cancellationToken
        )
        {
            var newContract = new Contract(
                0,
                request.Description,
                request.SupplierId,
                request.ContractTypeId,
                request.ContractStateId,
                request.ContractBaseValue,
                request.ContractValue,
                request.NumberOfWorkingHours,
                request.OwnEquipment,
                request.LeaderName,
                request.RemoteJob,
                request.BankId,
                request.BankAgency,
                request.BankAccount,
                request.PixKey,
                request.BusinessUnit,
                request.StartDate,
                request.EndDate,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newContract.AddEvent(new ContractCreatedEvent(newContract.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _contractDbContext.Contracts.AddAsync(
                        newContract,
                        cancellationToken: cancellationToken
                    );
                },
                newContract.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<Contract>.Success(newContract);
        }

        private Result<Response<ContractResponse>> GenerateResponse(Contract contract)
        {
            var contractResponse = mapper.Map<ContractResponse>(contract);
            var response = new Response<ContractResponse>(contractResponse);

            return Result<Response<ContractResponse>>.Success(response);
        }
    }
}
