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
    public class EditContractTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        ContractTypeDbContext contracttypeDbContext
    )
        : CommandHandler(contracttypeDbContext, mediator),
            IRequestHandler<EditContractTypeRequest, Result<Response<ContractTypeResponse>>>
    {
        private readonly ContractTypeDbContext _contracttypeDbContext = contracttypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<ContractTypeResponse>>> Handle(
            EditContractTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentContractTypeAsync(req.Id, cancellationToken))
                .BindAsync(currentContractType =>
                    EditAndSaveContractTypeAsync(currentContractType, request, cancellationToken)
                )
                .MapAsync(currentContractType =>
                {
                    return new Response<ContractTypeResponse>(null);
                });
        }

        private static Result<EditContractTypeRequest> ValidateRequest(
            EditContractTypeRequest request
        )
        {
            return request.Id != request.RequestId
                ? Result<EditContractTypeRequest>.Failure(
                    ContractTypeErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditContractTypeRequest>.Success(request);
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

        private async Task<Result<ContractType>> EditAndSaveContractTypeAsync(
            ContractType currentContractType,
            EditContractTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var editContractType = new ContractType(
                request.Id,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentContractType.CreatedAt.GetValueOrDefault(),
                currentContractType.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editContractType.AddEvent(new ContractTypeEditedEvent(editContractType.Id));

            await ExecuteTransactionAsync(
                () => _contracttypeDbContext.ContractTypes.Update(editContractType),
                editContractType.GetEvents(),
                cancellationToken
            );

            return Result<ContractType>.Success(editContractType);
        }
    }
}
