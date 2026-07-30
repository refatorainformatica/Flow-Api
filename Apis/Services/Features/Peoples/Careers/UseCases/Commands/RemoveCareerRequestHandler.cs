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
    public class RemoveCareerRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        CareerDbContext careerDbContext
    )
        : CommandHandler(careerDbContext, mediator),
            IRequestHandler<RemoveCareerRequest, Result<Response<CareerResponse>>>
    {
        private readonly CareerDbContext _careerDbContext = careerDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<CareerResponse>>> Handle(
            RemoveCareerRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentCareerAsync(req.Id, cancellationToken))
                .BindAsync(currentCareer => RemoveCareerAsync(currentCareer, cancellationToken))
                .MapAsync(currentCareer =>
                {
                    return new Response<CareerResponse>(null);
                });
        }

        private static Result<RemoveCareerRequest> ValidateRequest(RemoveCareerRequest request)
        {
            return request.Id == default
                ? Result<RemoveCareerRequest>.Failure(CareerErrors.NotFound(request.Id))
                : Result<RemoveCareerRequest>.Success(request);
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

        private async Task<Result<Career>> RemoveCareerAsync(
            Career removeCareer,
            CancellationToken cancellationToken
        )
        {
            removeCareer.DeletedAt = _dateTimeService.UtcNow;
            removeCareer.EditedAt = _dateTimeService.UtcNow;
            removeCareer.EditedBy = _authenticatedUserService.UserId;

            removeCareer.AddEvent(new CareerRemovedEvent(removeCareer.Id));

            await ExecuteTransactionAsync(
                () => _careerDbContext.Update(removeCareer),
                removeCareer.GetEvents(),
                cancellationToken
            );

            return Result<Career>.Success(removeCareer);
        }
    }
}
