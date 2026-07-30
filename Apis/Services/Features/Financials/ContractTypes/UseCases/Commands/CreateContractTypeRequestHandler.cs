using AutoMapper;
using MediatR;
using Services.Features.Financials.ContractTypes.Models;
using Services.Features.Financials.ContractTypes.Models.Events;
using Services.Features.Financials.ContractTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.ContractTypes.UseCases.Commands
{
    public class CreateContractTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        ContractTypeDbContext contracttypeDbContext
    )
        : CommandHandler(contracttypeDbContext, mediator),
            IRequestHandler<CreateContractTypeRequest, Result<Response<ContractTypeResponse>>>
    {
        private readonly ContractTypeDbContext _contracttypeDbContext = contracttypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<ContractTypeResponse>>> Handle(
            CreateContractTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveContractTypeAsync(request, cancellationToken)
                .BindAsync(contracttype => Task.FromResult(GenerateResponse(contracttype)));
        }

        private async Task<Result<ContractType>> SaveContractTypeAsync(
            CreateContractTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var newContractType = new ContractType(
                0,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newContractType.AddEvent(new ContractTypeCreatedEvent(newContractType.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _contracttypeDbContext.ContractTypes.AddAsync(
                        newContractType,
                        cancellationToken: cancellationToken
                    );
                },
                newContractType.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<ContractType>.Success(newContractType);
        }

        private Result<Response<ContractTypeResponse>> GenerateResponse(ContractType contracttype)
        {
            var contracttypeResponse = mapper.Map<ContractTypeResponse>(contracttype);
            var response = new Response<ContractTypeResponse>(contracttypeResponse);

            return Result<Response<ContractTypeResponse>>.Success(response);
        }
    }
}
