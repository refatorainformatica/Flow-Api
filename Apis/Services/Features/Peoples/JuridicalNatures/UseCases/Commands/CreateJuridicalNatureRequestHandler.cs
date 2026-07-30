using AutoMapper;
using MediatR;
using Services.Features.Peoples.JuridicalNatures.Models;
using Services.Features.Peoples.JuridicalNatures.Models.Events;
using Services.Features.Peoples.JuridicalNatures.Repositories;
using Services.Features.Peoples.JuridicalNatures.UseCases.Commands;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.JuridicalNatures.UseCases.Commands
{
    public class CreateJuridicalNatureRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        JuridicalNatureDbContext juridicalnatureDbContext
    )
        : CommandHandler(juridicalnatureDbContext, mediator),
            IRequestHandler<CreateJuridicalNatureRequest, Result<Response<JuridicalNatureResponse>>>
    {
        private readonly JuridicalNatureDbContext _juridicalnatureDbContext =
            juridicalnatureDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<JuridicalNatureResponse>>> Handle(
            CreateJuridicalNatureRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveJuridicalNatureAsync(request, cancellationToken)
                .BindAsync(juridicalnature => Task.FromResult(GenerateResponse(juridicalnature)));
        }

        private async Task<Result<JuridicalNature>> SaveJuridicalNatureAsync(
            CreateJuridicalNatureRequest request,
            CancellationToken cancellationToken
        )
        {
            var newJuridicalNature = new JuridicalNature(
                0,
                request.ExternalCode,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newJuridicalNature.AddEvent(new JuridicalNatureCreatedEvent(newJuridicalNature.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _juridicalnatureDbContext.JuridicalNatures.AddAsync(
                        newJuridicalNature,
                        cancellationToken: cancellationToken
                    );
                },
                newJuridicalNature.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<JuridicalNature>.Success(newJuridicalNature);
        }

        private Result<Response<JuridicalNatureResponse>> GenerateResponse(
            JuridicalNature juridicalnature
        )
        {
            var juridicalnatureResponse = mapper.Map<JuridicalNatureResponse>(juridicalnature);
            var response = new Response<JuridicalNatureResponse>(juridicalnatureResponse);

            return Result<Response<JuridicalNatureResponse>>.Success(response);
        }
    }
}
