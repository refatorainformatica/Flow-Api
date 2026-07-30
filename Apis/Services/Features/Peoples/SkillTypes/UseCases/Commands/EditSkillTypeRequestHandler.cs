using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.SkillTypes.Exceptions;
using Services.Features.Peoples.SkillTypes.Models;
using Services.Features.Peoples.SkillTypes.Models.Events;
using Services.Features.Peoples.SkillTypes.Repositories;
using Services.Features.Peoples.SkillTypes.UseCases.Commands;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.SkillTypes.UseCases.Commands
{
    public class EditSkillTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        SkillTypeDbContext skilltypeDbContext
    )
        : CommandHandler(skilltypeDbContext, mediator),
            IRequestHandler<EditSkillTypeRequest, Result<Response<SkillTypeResponse>>>
    {
        private readonly SkillTypeDbContext _skilltypeDbContext = skilltypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<SkillTypeResponse>>> Handle(
            EditSkillTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentSkillTypeAsync(req.Id, cancellationToken))
                .BindAsync(currentSkillType =>
                    EditAndSaveSkillTypeAsync(currentSkillType, request, cancellationToken)
                )
                .MapAsync(currentSkillType =>
                {
                    return new Response<SkillTypeResponse>(null);
                });
        }

        private static Result<EditSkillTypeRequest> ValidateRequest(EditSkillTypeRequest request)
        {
            return request.Id != request.RequestId
                ? Result<EditSkillTypeRequest>.Failure(
                    SkillTypeErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditSkillTypeRequest>.Success(request);
        }

        private async Task<Result<SkillType>> GetCurrentSkillTypeAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var skilltype = await _skilltypeDbContext
                .SkillTypes.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return skilltype is null
                ? Result<SkillType>.Failure(SkillTypeErrors.NotFound(id))
                : Result<SkillType>.Success(skilltype);
        }

        private async Task<Result<SkillType>> EditAndSaveSkillTypeAsync(
            SkillType currentSkillType,
            EditSkillTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var editSkillType = new SkillType(
                request.Id,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentSkillType.CreatedAt.GetValueOrDefault(),
                currentSkillType.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editSkillType.AddEvent(new SkillTypeEditedEvent(editSkillType.Id));

            await ExecuteTransactionAsync(
                () => _skilltypeDbContext.SkillTypes.Update(editSkillType),
                editSkillType.GetEvents(),
                cancellationToken
            );

            return Result<SkillType>.Success(editSkillType);
        }
    }
}
