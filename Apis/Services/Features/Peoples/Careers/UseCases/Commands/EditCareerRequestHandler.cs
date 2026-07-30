using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Careers.Exceptions;
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
    public class EditCareerRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        CareerDbContext careerDbContext
    )
        : CommandHandler(careerDbContext, mediator),
            IRequestHandler<EditCareerRequest, Result<Response<CareerResponse>>>
    {
        private readonly CareerDbContext _careerDbContext = careerDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<CareerResponse>>> Handle(
            EditCareerRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentCareerAsync(req.Id, cancellationToken))
                .BindAsync(currentCareer =>
                    EditAndSaveCareerAsync(currentCareer, request, cancellationToken)
                )
                .MapAsync(currentCareer =>
                {
                    return new Response<CareerResponse>(null);
                });
        }

        private static Result<EditCareerRequest> ValidateRequest(EditCareerRequest request)
        {
            return request.Id != request.RequestId
                ? Result<EditCareerRequest>.Failure(
                    CareerErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditCareerRequest>.Success(request);
        }

        private async Task<Result<Career>> GetCurrentCareerAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var career = await _careerDbContext
                .Careers.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return career is null
                ? Result<Career>.Failure(CareerErrors.NotFound(id))
                : Result<Career>.Success(career);
        }

        private async Task<Result<Career>> EditAndSaveCareerAsync(
            Career currentCareer,
            EditCareerRequest request,
            CancellationToken cancellationToken
        )
        {
            var editCareer = new Career(
                request.Id,
                request.ExternalCode,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentCareer.CreatedAt.GetValueOrDefault(),
                currentCareer.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editCareer.AddEvent(new CareerEditedEvent(editCareer.Id));

            await ExecuteTransactionAsync(
                () => _careerDbContext.Careers.Update(editCareer),
                editCareer.GetEvents(),
                cancellationToken
            );

            return Result<Career>.Success(editCareer);
        }
    }
}
