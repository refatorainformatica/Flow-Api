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
    public class EditContractRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        ContractDbContext contractDbContext
    )
        : CommandHandler(contractDbContext, mediator),
            IRequestHandler<EditContractRequest, Result<Response<ContractResponse>>>
    {
        private readonly ContractDbContext _contractDbContext = contractDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<ContractResponse>>> Handle(
            EditContractRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentContractAsync(req.Id, cancellationToken))
                .BindAsync(currentContract =>
                    EditAndSaveContractAsync(currentContract, request, cancellationToken)
                )
                .MapAsync(currentContract =>
                {
                    return new Response<ContractResponse>(null);
                });
        }

        private static Result<EditContractRequest> ValidateRequest(EditContractRequest request)
        {
            return request.Id != request.RequestId
                ? Result<EditContractRequest>.Failure(
                    ContractErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditContractRequest>.Success(request);
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

        private async Task<Result<Contract>> EditAndSaveContractAsync(
            Contract currentContract,
            EditContractRequest request,
            CancellationToken cancellationToken
        )
        {
            var editContract = new Contract(
                request.Id,
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
                currentContract.CreatedAt.GetValueOrDefault(),
                currentContract.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editContract.AddEvent(new ContractEditedEvent(editContract.Id));

            await ExecuteTransactionAsync(
                () => _contractDbContext.Contracts.Update(editContract),
                editContract.GetEvents(),
                cancellationToken
            );

            return Result<Contract>.Success(editContract);
        }
    }
}
