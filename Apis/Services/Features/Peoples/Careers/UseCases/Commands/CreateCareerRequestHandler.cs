using AutoMapper;
using MediatR;
using Services.Features.Peoples.Careers.Models;
using Services.Features.Peoples.Careers.Models.Events;
using Services.Features.Peoples.Careers.Repositories;
using Services.Features.Peoples.Careers.UseCases.Commands;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.Careers.UseCases.Commands
{
    public class CreateCareerRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        CareerDbContext careerDbContext
    )
        : CommandHandler(careerDbContext, mediator),
            IRequestHandler<CreateCareerRequest, Result<Response<CareerResponse>>>
    {
        private readonly CareerDbContext _careerDbContext = careerDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<CareerResponse>>> Handle(
            CreateCareerRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveCareerAsync(request, cancellationToken)
                .BindAsync(career => Task.FromResult(GenerateResponse(career)));
        }

        private async Task<Result<Career>> SaveCareerAsync(
            CreateCareerRequest request,
            CancellationToken cancellationToken
        )
        {
            var newCareer = new Career(
                0,
                request.ExternalCode,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newCareer.AddEvent(new CareerCreatedEvent(newCareer.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _careerDbContext.Careers.AddAsync(
                        newCareer,
                        cancellationToken: cancellationToken
                    );
                },
                newCareer.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<Career>.Success(newCareer);
        }

        private Result<Response<CareerResponse>> GenerateResponse(Career career)
        {
            var careerResponse = mapper.Map<CareerResponse>(career);
            var response = new Response<CareerResponse>(careerResponse);

            return Result<Response<CareerResponse>>.Success(response);
        }
    }
}
