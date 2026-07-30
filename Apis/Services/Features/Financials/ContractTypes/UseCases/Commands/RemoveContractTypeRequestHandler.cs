using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.ContractTypes.Exceptions;
using Services.Features.Financials.ContractTypes.Models;
using Services.Features.Financials.ContractTypes.Models.Events;
using Services.Features.Financials.ContractTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.ContractTypes.UseCases.Commands
{
    public class RemoveContractTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        ContractTypeDbContext contracttypeDbContext
    )
        : CommandHandler(contracttypeDbContext, mediator),
            IRequestHandler<RemoveContractTypeRequest, Result<Response<ContractTypeResponse>>>
    {
        private readonly ContractTypeDbContext _contracttypeDbContext = contracttypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<ContractTypeResponse>>> Handle(
            RemoveContractTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentContractTypeAsync(req.Id, cancellationToken))
                .BindAsync(currentContractType =>
                    RemoveContractTypeAsync(currentContractType, cancellationToken)
                )
                .MapAsync(currentContractType =>
                {
                    return new Response<ContractTypeResponse>(null);
                });
        }

        private static Result<RemoveContractTypeRequest> ValidateRequest(
            RemoveContractTypeRequest request
        )
        {
            return request.Id == default
                ? Result<RemoveContractTypeRequest>.Failure(ContractTypeErrors.NotFound(request.Id))
                : Result<RemoveContractTypeRequest>.Success(request);
        }

        private async Task<Result<ContractType>> GetCurrentContractTypeAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var contracttype = await _contracttypeDbContext
                .ContractTypes.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return contracttype is null
                ? Result<ContractType>.Failure(ContractTypeErrors.NotFound(id))
                : Result<ContractType>.Success(contracttype);
        }

        private async Task<Result<ContractType>> RemoveContractTypeAsync(
            ContractType removeContractType,
            CancellationToken cancellationToken
        )
        {
            removeContractType.DeletedAt = _dateTimeService.UtcNow;
            removeContractType.EditedAt = _dateTimeService.UtcNow;
            removeContractType.EditedBy = _authenticatedUserService.UserId;

            removeContractType.AddEvent(new ContractTypeRemovedEvent(removeContractType.Id));

            await ExecuteTransactionAsync(
                () => _contracttypeDbContext.Update(removeContractType),
                removeContractType.GetEvents(),
                cancellationToken
            );

            return Result<ContractType>.Success(removeContractType);
        }
    }
}
